using Microsoft.AspNetCore.Identity;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.ErrorCatalog;

namespace MixxFit.API.Features.Auth.ResetPassword;

public class ResetPasswordHandler(UserManager<User> userManager, ITokenService tokenService) : IHandler
{
    public async Task<Result> Handle(ResetPasswordRequest request)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure(AuthError.InvalidPasswordResetToken($"User with id |{request.UserId}| has not been found during password reset."));

        var resetResult = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!resetResult.Succeeded)
            return Result.Failure(AuthError.InvalidPasswordResetToken($"Password reset failed for user with id |{request.UserId}|."));

        await tokenService.RevokeAllRefreshTokens(user.Id);

        return Result.Success();
    }
}
