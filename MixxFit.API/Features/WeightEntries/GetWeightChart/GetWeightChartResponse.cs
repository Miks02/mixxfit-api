using MixxFit.API.Features.WeightEntries.Shared;

namespace MixxFit.API.Features.WeightEntries.GetWeightChart;

public record GetWeightChartResponse
{
    public IReadOnlyList<WeightRecordDto> Entries { get; init; } = [];
    public double? TargetWeight { get; init; }
}