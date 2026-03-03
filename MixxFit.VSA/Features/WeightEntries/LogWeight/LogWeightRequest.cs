using Microsoft.AspNetCore.Mvc;

namespace MixxFit.VSA.Features.WeightEntries.LogWeight;

public record LogWeightRequest
{
    public double Weight { get; set; }
    public TimeSpan Time { get; set; }
    public string? Notes { get; set; }
}