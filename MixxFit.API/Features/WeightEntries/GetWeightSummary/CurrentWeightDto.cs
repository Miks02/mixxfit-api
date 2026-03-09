namespace MixxFit.API.Features.WeightEntries.GetWeightSummary;

public record CurrentWeightDto
{
    public double? Weight { get; set; }
    public DateTime? CreatedAt { get; set; }
}