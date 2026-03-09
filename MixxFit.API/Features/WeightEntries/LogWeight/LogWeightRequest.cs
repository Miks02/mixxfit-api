using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Features.WeightEntries.LogWeight;

public record LogWeightRequest
{
    public double Weight { get; set; }
    public TimeSpan Time { get; set; }
    public string? Notes { get; set; }
}