using WorkoutTrackerApi.DTO.Auth;
using WorkoutTrackerApi.DTO.User;
using WorkoutTrackerApi.Models;
using WorkoutTrackerApi.Services.Results;

namespace WorkoutTrackerApi.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);
    
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    
    Task<ServiceResult> LogoutAsync(string refreshToken);
    
    Task<ServiceResult<AuthResponseDto>> RotateAuthTokens(string refreshToken);
}
