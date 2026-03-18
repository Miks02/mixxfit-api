using FluentValidation;

namespace MixxFit.API.Features.Exercises.CreateExercise;

public class CreateExerciseValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty")
            .MaximumLength(50)
            .WithMessage("Name cannot cannot exceed 50 characters");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category ID must be greater than 0");

        RuleFor(x => x.MuscleGroupId)
            .GreaterThan(0)
            .WithMessage("Muscle Group ID must be greater than 0");

    }
}