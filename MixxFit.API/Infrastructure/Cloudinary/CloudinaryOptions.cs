namespace MixxFit.API.Infrastructure.Cloudinary;

public record CloudinaryOptions
{
    public string CloudName { get; init; } = null!;
    public string ApiKey { get; init; } = null!;
    public string ApiSecret { get; init; } = null!;
};