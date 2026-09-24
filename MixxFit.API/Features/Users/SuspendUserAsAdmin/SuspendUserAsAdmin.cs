using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Users.SuspendUserAsAdmin;

public static class SuspendUserAsAdmin
{
    public class SuspendUserAsAdminHandler(
        UserManager<User> userManager,
        ITokenService tokenService,
        IAuthEmailSender emailSender) : IHandler
    {
        public async Task<Result> Handle(string userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result.Failure(UserError.NotFound(userId));

            if (user.AccountStatus == AccountStatus.Suspended)
                return Result.Failure(UserError.UserAlreadySuspended(userId));

            user.AccountStatus = AccountStatus.Suspended;

            var updateResult = (await userManager.UpdateAsync(user)).HandleIdentityResult();

            if (!updateResult.IsSuccess)
                return updateResult;
            
            await emailSender.SendAccountDeactivatedEmailAsync(user.Email!);

            await tokenService.RevokeAllRefreshTokens(user.Id);

            return Result.Success();
        }
    }

    public class SuspendUserAsAdminEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("admin/users/{userId}/suspend", async (
                string userId,
                SuspendUserAsAdminHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(userId, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);
        }
    }
}
