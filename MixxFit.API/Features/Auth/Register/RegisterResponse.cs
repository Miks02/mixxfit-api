using System.Text.Json.Serialization;
using MixxFit.API.Features.Common;

namespace MixxFit.API.Features.Auth.Register;

public record RegisterResponse(string AccessToken, [property: JsonIgnore] string RefreshToken, UserDetailsDto User);
