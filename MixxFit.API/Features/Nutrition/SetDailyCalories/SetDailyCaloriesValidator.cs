using FluentValidation;

namespace MixxFit.API.Features.Nutrition.SetDailyCalories;

public class SetDailyCaloriesValidator : AbstractValidator<SetDailyCaloriesRequest>
{
    public SetDailyCaloriesValidator()
    {
        RuleFor(x => x.Calories)
            .NotEmpty()
            .WithMessage("Calories are required")
            .InclusiveBetween(1000, 10000)
            .WithMessage("Calories must be between 1000 and 10000");
            
    }
}