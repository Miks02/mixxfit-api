using MixxFit.VSA.Features.WeightEntries.Shared;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightLogs;

public record GetWeightLogsResponse
{
    public IReadOnlyList<WeightRecordDto> WeightLogs { get; init; } = [];
    public IReadOnlyList<int> Months { get; init; } = [];
};