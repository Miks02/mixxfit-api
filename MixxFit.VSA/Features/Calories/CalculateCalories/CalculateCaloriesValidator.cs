using FluentValidation;

namespace MixxFit.VSA.Features.Calories.CalculateCalories;

public class CalculateCaloriesValidator : AbstractValidator<CalculateCaloriesRequest>
{
    public CalculateCaloriesValidator()
    {
        RuleFor(x => x.Age)
            .NotEmpty()
            .WithMessage("Age is required")
            .InclusiveBetween(13, 130)
            .WithMessage("Age must be between 13 and 130 years old.");
        
        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("Gender is required")
            .IsInEnum()
            .WithMessage("Invalid gender enum.");
        
        RuleFor(x => x.Height)
            .NotEmpty()
            .WithMessage("Height is required")
            .InclusiveBetween(70, 250)
            .WithMessage("Height must be between 70 and 250 cm.");
        
        RuleFor(x => x.Weight)
            .NotEmpty()
            .WithMessage("Weight is required")
            .InclusiveBetween(25, 400)
            .WithMessage("Weight must be between 25 and 400 kg.");
        
        RuleFor(x => x.ActivityLevel)
            .NotEmpty()
            .WithMessage("Activity level is required")
            .IsInEnum()
            .WithMessage("Invalid activity level enum.");
    }
}