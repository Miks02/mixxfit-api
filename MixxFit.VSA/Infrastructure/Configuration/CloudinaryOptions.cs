namespace MixxFit.VSA.Infrastructure.Configuration;

public record CloudinaryOptions
{
    public string CloudName { get; init; } = null!;
    public string ApiKey { get; init; } = null!;
    public string ApiSecret { get; init; } = null!;
};