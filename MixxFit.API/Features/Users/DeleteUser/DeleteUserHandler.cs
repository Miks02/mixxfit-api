using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Users.DeleteUser;

public class DeleteUserHandler(
    UserManager<User> userManager,
    AppDbContext context,
    ITokenService tokenService,
    IFileService fileService,
    IAuthEmailSender emailSender) : IHandler
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

        var email = user.Email!;

        var result = await HasRelevantDataAsync(userId, cancellationToken)
            ? await AnonymizeAsync(user)
            : await HardDeleteAsync(user);

        if (!result.IsSuccess)
            return result;

        await emailSender.SendAccountDeletedEmailAsync(email);

        return Result.Success();
    }

    private async Task<bool> HasRelevantDataAsync(string userId, CancellationToken cancellationToken)
    {
        return await context.Workouts.AnyAsync(w => w.OwnerId == userId, cancellationToken)
               || await context.WeightEntries.AnyAsync(w => w.OwnerId == userId, cancellationToken)
               || await context.Exercises.IgnoreQueryFilters().AnyAsync(e => e.OwnerId == userId, cancellationToken);
    }

    private async Task<Result> HardDeleteAsync(User user)
    {
        var deleteFileResult = await DeleteImageAsync(user.ImagePath);

        if (!deleteFileResult.IsSuccess)
            return deleteFileResult;

        return (await userManager.DeleteAsync(user)).HandleIdentityResult();
    }

    private async Task<Result> AnonymizeAsync(User user)
    {
        var deleteFileResult = await DeleteImageAsync(user.ImagePath);

        if (!deleteFileResult.IsSuccess)
            return deleteFileResult;

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

    private async Task<Result> DeleteImageAsync(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return Result.Success();

        var deleteResult = await fileService.DeleteFile(imagePath);

        return deleteResult.IsSuccess
            ? Result.Success()
            : Result.Failure(deleteResult.Errors.ToArray());
    }
}
