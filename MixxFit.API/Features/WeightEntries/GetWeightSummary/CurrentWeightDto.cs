namespace MixxFit.API.Features.WeightEntries.GetWeightSummary;

public record CurrentWeightDto
{
    public decimal Weight { get; set; }
    public DateTime CreatedAt { get; set; }
}