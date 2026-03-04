using MixxFit.VSA.Domain.Enums;

namespace MixxFit.VSA.Features.Nutrition.CalculateCalories;

public record CalculateCaloriesRequest
{
    public int Age { get; init; }
    public double Height { get; init; }
    public double Weight { get; init; }
    public Gender Gender { get; init; }
    public ActivityLevel ActivityLevel { get; init; }
};