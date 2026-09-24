using Microsoft.AspNetCore.Identity;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Auth.SendResetPasswordLink;

public class SendResetPasswordLinkHandler(
    UserManager<User> userManager,
    IAuthEmailSender authEmailSender) : IHandler
{
    public async Task<Result> Handle(SendResetPasswordLinkRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null || user.AccountStatus != AccountStatus.Active)
            return Result.Success();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        await authEmailSender.SendPasswordResetEmailAsync(user.Email!, user.Id, token);
        return Result.Success();
    }
}
