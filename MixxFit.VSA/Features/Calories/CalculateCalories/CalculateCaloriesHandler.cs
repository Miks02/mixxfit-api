using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Domain.Enums;

namespace MixxFit.VSA.Features.Calories.CalculateCalories;

public class CalculateCaloriesHandler : IHandler
{
    public CalculateCaloriesResponse Handle(CalculateCaloriesRequest request)
    {
        var baseBmr = 10 * request.Weight + 6.25 * request.Height - 5 * request.Age;

        var bmr = request.Gender switch
        {
            Gender.Male => baseBmr + 5,
            Gender.Female => baseBmr - 161,
            _ => baseBmr - 78 
        };

        Dictionary<ActivityLevel, double> activityMultiplier = new Dictionary<ActivityLevel, double>
        {
            [ActivityLevel.Sedentary] = 1.2,
            [ActivityLevel.Light] = 1.375,
            [ActivityLevel.Moderate] = 1.55,
            [ActivityLevel.Active] = 1.725,
            [ActivityLevel.VeryActive] = 1.9
        };
        
        var maintenance = Math.Round(bmr * activityMultiplier[request.ActivityLevel]);

        return new CalculateCaloriesResponse
        {
            Bmr = bmr,
            Maintenance = maintenance,
            MildGain = maintenance + 250,
            MildLoss = maintenance - 250,
            AggressiveGain = maintenance + 500,
            AggressiveLoss = maintenance - 500,
        };

    }   
}