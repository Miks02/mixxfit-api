using System.Text.Json.Serialization;

namespace MixxFit.API.Features.Auth.RotateTokens;

public record RotateTokensResponse(string AccessToken, [property: JsonIgnore]string RefreshToken);