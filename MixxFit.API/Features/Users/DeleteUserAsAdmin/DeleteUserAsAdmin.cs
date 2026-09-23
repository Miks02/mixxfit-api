using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Users.DeleteUser;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Users.DeleteUserAsAdmin;

public static class DeleteUserAsAdmin
{
    public class DeleteUserAsAdminHandler(
        UserManager<User> userManager,
        AppDbContext context,
        ITokenService tokenService,
        IFileService fileService,
        DeleteUserHandler deleteUserHandler) : IHandler
    {
        public async Task<Result> Handle(string userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Include(u => u.FitnessProfile)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
                return Result.Failure(UserError.NotFound(userId));

            if (user.DeletedAt is not null)
                return Result.Failure(UserError.UserAlreadyDeleted(userId));

            if (!await HasRelevantDataAsync(userId, cancellationToken))
                return await deleteUserHandler.Handle(userId, cancellationToken);

            return await AnonymizeAsync(user, cancellationToken);
        }

        private async Task<bool> HasRelevantDataAsync(string userId, CancellationToken cancellationToken)
        {
            return await context.Workouts.AnyAsync(w => w.OwnerId == userId, cancellationToken)
                   || await context.WeightEntries.AnyAsync(w => w.OwnerId == userId, cancellationToken)
                   || await context.Exercises.IgnoreQueryFilters().AnyAsync(e => e.OwnerId == userId, cancellationToken);
        }

        private async Task<Result> AnonymizeAsync(User user, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(user.ImagePath))
            {
                var deleteResult = await fileService.DeleteFile(user.ImagePath);

                if (!deleteResult.IsSuccess)
                    return Result.Failure(deleteResult.Errors.ToArray());
            }

            var idPart = user.Id.Replace("-", "");

            user.FirstName = "Deleted";
            user.LastName = "Deleted";
            user.UserName = $"deleted_{idPart[..Math.Min(12, idPart.Length)]}";
            user.Email = $"deleted_{user.Id}@deleted.invalid";
            user.EmailConfirmed = false;
            user.PhoneNumber = null;
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.ImagePath = null;
            user.PasswordHash = null;
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.AccountStatus = AccountStatus.Deleted;
            user.DeletedAt = DateTime.UtcNow;

            user.FitnessProfile.Gender = null;
            user.FitnessProfile.Height = null;
            user.FitnessProfile.Weight = null;
            user.FitnessProfile.TargetWeight = null;
            user.FitnessProfile.DailyCalorieGoal = null;
            user.FitnessProfile.DateOfBirth = null;

            var updateResult = (await userManager.UpdateAsync(user)).HandleIdentityResult();

            if (!updateResult.IsSuccess)
                return updateResult;

            var roles = await userManager.GetRolesAsync(user);

            if (roles.Count > 0)
            {
                var rolesResult = (await userManager.RemoveFromRolesAsync(user, roles)).HandleIdentityResult();

                if (!rolesResult.IsSuccess)
                    return rolesResult;
            }

            await tokenService.RevokeAllRefreshTokens(user.Id);

            return Result.Success();
        }
    }

    public class DeleteUserAsAdminEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("admin/users/{userId}", async (
                string userId,
                DeleteUserAsAdminHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(userId, cancellationToken);
                return result.ToTypedResult(HttpStatusCode.NoContent);
            })
            .WithTags("Users")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);
        }
    }
}
