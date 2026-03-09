using MixxFit.API.Features.WeightEntries.Shared;

namespace MixxFit.API.Features.WeightEntries.GetWeightSummary;

public record WeightListDetails
{
    public IReadOnlyList<WeightRecordDto> WeightLogs { get; set; } = [];
    public IReadOnlyList<int> Months { get; set; } = [];
}