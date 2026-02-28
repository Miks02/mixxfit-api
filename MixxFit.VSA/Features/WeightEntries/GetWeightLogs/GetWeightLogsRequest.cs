using Microsoft.AspNetCore.Mvc;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightLogs;

public record GetWeightLogsRequest
{
    [FromQuery(Name = "Year")]
    public int? Year { get; init; }
    [FromQuery(Name = "Month")]
    public int? Month { get; init; }
};