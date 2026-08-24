using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.RefreshTokens;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Infrastructure.Security;

namespace MixxFit.API.Common.Interfaces;

public interface ITokenService
{
    Task<TokenResponseDto> GenerateAuthTokens(User user);
    Task<string> GenerateJwtToken(User user);
    Task<Result> RevokeRefreshToken(string oldToken);
    Task RevokeAllRefreshTokens(string userId);
    string CreateRefreshToken();
    string HashToken(string rawToken);
    Result<string> ValidateRefreshToken(RefreshToken? oldToken);
}