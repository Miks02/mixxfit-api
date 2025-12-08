using FluentValidation;
using WorkoutTrackerApi.DTO.Workout;

namespace WorkoutTrackerApi.Validators;

public class WorkoutCreateRequestValidator : AbstractValidator<WorkoutCreateRequest>
{
    public WorkoutCreateRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Workout name is required")
            .MaximumLength(200).WithMessage("Name is too long (200 characters max)");

        RuleFor(p => p.Notes)
            .MaximumLength(500).WithMessage("Notes are too long (500 characters max)")
            .When(p => !string.IsNullOrEmpty(p.Notes));

        RuleForEach(p => p.ExerciseEntries)
            .SetValidator(new ExerciseEntryDtoValidator());

    }
}