namespace MixxFit.VSA.Features.WeightEntries.GetWeightSummary;

public record WeightRecordDto
{
    public int Id { get; set; }
    public double Weight { get; set; }
    public TimeSpan TimeLogged { get; set; }
    public DateTime CreatedAt { get; set; }
}