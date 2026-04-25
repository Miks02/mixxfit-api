using FluentValidation;

namespace MixxFit.API.Features.WorkoutTemplates.EditTemplate;

public class ExerciseItemValidator : AbstractValidator<EditTemplateRequest.ExerciseItem>
{
    public ExerciseItemValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotEmpty()
            .WithMessage("Exercise is required");

        RuleFor(x => x.SetCount)
            .GreaterThan(0)
            .WithMessage("Set count must be greater than 0")
            .LessThanOrEqualTo(50)
            .WithMessage("Set count must be less than or equal to 50");
    }
}