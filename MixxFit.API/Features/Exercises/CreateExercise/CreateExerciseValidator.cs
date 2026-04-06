using FluentValidation;

namespace MixxFit.API.Features.Exercises.CreateExercise;

public class CreateExerciseValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Exercise name cannot be empty")
            .MinimumLength(4)
            .WithMessage("Exercise name must be at least 4 characters long")
            .MaximumLength(50)
            .WithMessage("Exercise name cannot cannot exceed 50 characters");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category ID must be greater than 0")
            .NotEqual(5)
            .WithMessage("Category that has an ID of 5 is of type 'Other'. Which is not permitted for creating custom exercises");

        RuleFor(x => x.MuscleGroupId)
            .GreaterThan(0)
            .WithMessage("Muscle Group ID must be greater than 0");

    }
}