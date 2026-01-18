using System.Text.Json.Serialization;
using VitalOps.API.DTO.User;

namespace VitalOps.API.DTO.Auth;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = null!;
    
    [JsonIgnore]
    public string RefreshToken { get; set; } = null!;
    
    public UserDetailsDto? User { get; set; }
}