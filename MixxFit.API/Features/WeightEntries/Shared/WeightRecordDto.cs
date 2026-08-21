namespace MixxFit.API.Features.WeightEntries.Shared;

public record WeightRecordDto
{
    public int Id { get; init; }
    public decimal Weight { get; init; }
    public string? Notes { get; init; }
    public TimeSpan TimeLogged { get; init; }
    public DateTime CreatedAt { get; init; }
}