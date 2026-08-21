namespace MixxFit.API.Features.WeightEntries.GetWeightSummary;

public record WeightDeltaDto
{
    public decimal Delta { get; set; }
    public DateTime CreatedAt { get; set; }
};