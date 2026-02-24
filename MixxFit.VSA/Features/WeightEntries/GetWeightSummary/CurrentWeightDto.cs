namespace MixxFit.VSA.Features.WeightEntries.GetWeightSummary;

public record CurrentWeightDto
{
    public double? Weight { get; set; }
    public DateTime? CreatedAt { get; set; }
}