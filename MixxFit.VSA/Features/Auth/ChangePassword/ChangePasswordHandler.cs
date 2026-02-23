using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;

namespace MixxFit.VSA.Features.Auth.ChangePassword;

public class ChangePasswordHandler(UserManager<User> userManager) : IHandler
{
    public async Task<Result> Handle(string userId, ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Failure(UserError.NotFound(userId));

        var changePasswordResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!changePasswordResult.Succeeded)
            return Result.Failure(changePasswordResult.Errors.ToArray());

        user.RefreshToken = null;
        user.TokenExpDate = null;
        
        var logoutResult = await userManager.UpdateAsync(user);
        
        if(!logoutResult.Succeeded)
            return Result.Failure(logoutResult.Errors.ToArray());
        
        return Result.Success();
    }
}