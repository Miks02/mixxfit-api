namespace MixxFit.VSA.Features.WeightEntries.LogWeight;

public record LogWeightResponse
{
    public int Id { get; set; }
    public double Weight { get; set; }
    public TimeSpan Time { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Notes { get; set; }
}