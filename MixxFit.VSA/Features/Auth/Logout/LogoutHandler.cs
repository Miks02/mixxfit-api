using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;

namespace MixxFit.VSA.Features.Auth.Logout;

public class LogoutHandler(UserManager<User> userManager) : IHandler
{
    public async Task<Result> Handle(LogoutRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            return Result.Failure(AuthError.JwtError("Refresh token is missing"));

        var user = await userManager.Users
            .Where(u => u.RefreshToken == request.RefreshToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Success();

        user.RefreshToken = null;
        user.TokenExpDate = null;
        
        var removeTokenResult = await userManager.UpdateAsync(user);
        
        return removeTokenResult.Succeeded ? Result.Success() : Result.Failure(removeTokenResult.Errors.ToArray());
    }
}