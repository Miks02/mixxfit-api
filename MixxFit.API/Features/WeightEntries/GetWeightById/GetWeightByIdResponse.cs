namespace MixxFit.API.Features.WeightEntries.GetWeightById;

public record GetWeightByIdResponse
{
    public int Id { get; set; }
    public double Weight { get; set; }
    public TimeSpan Time { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Notes { get; set; }
}