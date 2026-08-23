using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;

namespace MixxFit.API.Features.Auth.Logout;

public class LogoutHandler(ITokenService tokenService) : IHandler
{
    public async Task<Result> Handle(LogoutRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            return Result.Failure(AuthError.JwtError("Refresh token is missing"));

        var revokeOldTokenResult = await tokenService.RevokeRefreshToken(request.RefreshToken);
        
        return revokeOldTokenResult;
    }
}