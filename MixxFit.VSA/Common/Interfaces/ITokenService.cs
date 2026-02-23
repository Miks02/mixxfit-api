using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Infrastructure.Security;

namespace MixxFit.VSA.Common.Interfaces;

public interface ITokenService
{
    Task<Result<TokenResponseDto>> GenerateAuthTokens(User user);
}