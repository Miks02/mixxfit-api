using System.Text.Json.Serialization;
using MixxFit.API.Features.Common;

namespace MixxFit.API.Features.Auth.Login;

public record LoginResponse(string AccessToken, [property: JsonIgnore] string RefreshToken, UserDetailsDto User);
