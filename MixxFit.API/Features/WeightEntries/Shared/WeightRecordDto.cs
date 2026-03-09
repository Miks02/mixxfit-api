namespace MixxFit.API.Features.WeightEntries.Shared;

public record WeightRecordDto
{
    public int Id { get; init; }
    public double Weight { get; init; }
    public TimeSpan TimeLogged { get; init; }
    public DateTime CreatedAt { get; init; }
}