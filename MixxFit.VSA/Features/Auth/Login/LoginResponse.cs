using System.Text.Json.Serialization;
using MixxFit.VSA.Features.Common;

namespace MixxFit.VSA.Features.Auth.Login;

public record LoginResponse(string AccessToken, [property: JsonIgnore] string RefreshToken, UserDetailsDto User);
