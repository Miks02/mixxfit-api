using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;

namespace MixxFit.API.Features.Auth.Logout;

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