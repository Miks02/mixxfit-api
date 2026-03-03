namespace MixxFit.VSA.Features.Calories.CalculateCalories;

public record CalculateCaloriesResponse
{
    public double Bmr { get; init; }
    public double AggressiveLoss { get; init; }
    public double MildLoss { get; init; }
    public double Maintenance { get; init; }
    public double MildGain { get; init; }
    public double AggressiveGain { get; init; }
};