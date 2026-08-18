using MixxFit.API.Features.WeightEntries.Shared;

namespace MixxFit.API.Features.WeightEntries.GetWeightSummary;

public record GetWeightSummaryResponse
{
    public CurrentWeightDto CurrentWeight { get; init; } = null!;
    public WeightListDetails WeightListDetails { get; init; } = null!;
    public WeightChartDto WeightChart { get; init; } = null!;
    public WeightDeltaDto? WeightDelta { get; init; }
    public Dictionary<int, IEnumerable<int>> YearsAndMonthsGroup { get; init; } = [];
}