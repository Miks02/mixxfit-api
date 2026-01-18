using VitalOps.API.DTO.Auth;
using VitalOps.API.Services.Results;
using VitalOps.API.DTO.User;
using VitalOps.API.Models;

namespace VitalOps.API.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    
    Task<ServiceResult> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
    
    Task<ServiceResult<AuthResponseDto>> RotateAuthTokens(string refreshToken, CancellationToken cancellationToken = default);
}
