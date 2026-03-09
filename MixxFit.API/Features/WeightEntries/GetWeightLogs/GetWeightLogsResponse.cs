using MixxFit.API.Features.WeightEntries.Shared;

namespace MixxFit.API.Features.WeightEntries.GetWeightLogs;

public record GetWeightLogsResponse
{
    public IReadOnlyList<WeightRecordDto> WeightLogs { get; init; } = [];
    public IReadOnlyList<int> Months { get; init; } = [];
};