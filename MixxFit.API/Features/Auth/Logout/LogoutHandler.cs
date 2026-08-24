using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Auth.Logout;

public class LogoutHandler(ITokenService tokenService, ILogger<LogoutHandler> logger) : IHandler
{
    public async Task Handle(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            logger.LogInformation("No refresh token provided for logout.");
            return;
        }

        await tokenService.RevokeRefreshToken(refreshToken);
        
    }
}