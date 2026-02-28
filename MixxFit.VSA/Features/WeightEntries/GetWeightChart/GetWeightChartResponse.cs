using MixxFit.VSA.Features.WeightEntries.Shared;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightChart;

public record GetWeightChartResponse
{
    public IReadOnlyList<WeightRecordDto> Entries { get; init; } = [];
    public double? TargetWeight { get; init; }
}