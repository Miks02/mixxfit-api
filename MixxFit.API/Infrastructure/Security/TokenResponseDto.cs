namespace MixxFit.API.Infrastructure.Security;

public record TokenResponseDto(string AccessToken, string RefreshToken);
