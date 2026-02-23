using System.Text.Json.Serialization;

namespace MixxFit.VSA.Features.Auth.Login;

public record LoginResponse(string AccessToken, [property: JsonIgnore] string RefreshToken, UserDetailsDto User);
