using FluentValidation;

namespace MixxFit.API.Features.WorkoutTemplates.CreateTemplate;

public class CreateTemplateValidator : AbstractValidator<CreateTemplateRequest>
{
    public CreateTemplateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Template name cannot be empty")
            .MinimumLength(4)
            .MaximumLength(100)
            .WithMessage("Template name must be between 4 and 100 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(200)
            .WithMessage("Notes cannot exceed 200 characters");
        
        RuleFor(x => x.Exercises)
            .NotEmpty()
            .WithMessage("Template must contain at least one exercise");
        
        RuleFor(x => x.Exercises)
            .Must(x => x.Count <= 50)
            .WithMessage("Template cannot contain more than 50 exercises");
        
        RuleForEach(x => x.Exercises)
            .SetValidator(new ExerciseItemValidator());  
        
    }
}