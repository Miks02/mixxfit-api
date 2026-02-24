namespace MixxFit.VSA.Features.WeightEntries.GetWeightSummary;

public record WeightChartDto
{
    public IReadOnlyList<WeightRecordDto> Entries { get; set; } = [];
    public double? TargetWeight { get; set; }
}