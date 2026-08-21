using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Features.WeightEntries.LogWeight;

public record LogWeightRequest
{
    public decimal Weight { get; set; }
    public TimeSpan Time { get; set; }
    public string? Notes { get; set; }
}