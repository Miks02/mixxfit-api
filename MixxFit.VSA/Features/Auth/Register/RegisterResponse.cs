using System.Text.Json.Serialization;

namespace MixxFit.VSA.Features.Auth.Register;

public record RegisterResponse(string AccessToken, [property: JsonIgnore] string RefreshToken, UserDetailsDto User);
