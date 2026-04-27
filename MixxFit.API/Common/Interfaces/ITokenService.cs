using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Infrastructure.Security;

namespace MixxFit.API.Common.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponseDto>> GenerateAuthTokens(User user);
}