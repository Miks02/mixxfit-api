using Microsoft.AspNetCore.Mvc;

namespace MixxFit.VSA.Features.WeightEntries.LogWeight;

public record LogWeightRequest
{
    [FromQuery(Name = "weight")]
    public double Weight { get; set; }
    [FromQuery(Name = "time")]
    public TimeSpan Time { get; set; }
    [FromQuery(Name = "notes")]
    public string? Notes { get; set; }
}