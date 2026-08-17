namespace MixxFit.API.Features.WeightEntries.GetWeightSummary;

public record WeightDeltaDto
{
    public double Delta { get; set; }
    public DateTime CreatedAt { get; set; }
};