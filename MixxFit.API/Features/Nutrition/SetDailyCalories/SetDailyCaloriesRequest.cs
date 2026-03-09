using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Features.Nutrition.SetDailyCalories;

public record SetDailyCaloriesRequest(double Calories);