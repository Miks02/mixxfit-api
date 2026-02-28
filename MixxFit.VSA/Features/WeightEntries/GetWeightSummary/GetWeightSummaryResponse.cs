using MixxFit.VSA.Features.WeightEntries.Shared;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightSummary;

public record GetWeightSummaryResponse
{
    public WeightRecordDto FirstEntry { get; set; } = null!;
    public CurrentWeightDto CurrentWeight { get; set; } = null!;
    public double Progress { get; set; }
    public WeightListDetails WeightListDetails { get; set; } = null!;
    public WeightChartDto WeightChart { get; set; } = null!;
    public IReadOnlyList<int> Years { get; set; } = [];
}