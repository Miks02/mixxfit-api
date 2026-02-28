using Microsoft.AspNetCore.Mvc;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightSummary;

public record GetWeightSummaryRequest
{
    [FromQuery(Name = "month")]
    public int? Month { get; set; }
    
    [FromQuery(Name = "year")]
    public int? Year { get; set; }
    
    [FromQuery(Name = "targetWeight")]
    public double? TargetWeight { get; set; }
}