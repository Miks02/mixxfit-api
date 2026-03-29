using FluentValidation;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.CreateWorkout;

public class SetEntryValidator : AbstractValidator<SetEntryDto>
{
    public SetEntryValidator(ExerciseType exerciseType)
    {
        RuleFor(p => p.Reps)
            .GreaterThan(0)
            .WithMessage("At least 1 repetition is required")
            .LessThan(1001)
            .WithMessage("Number of reps can be between 1 and 1000");

        RuleFor(p => p.Weight)
            .GreaterThan(0)
            .WithMessage("Weight is required")
            .LessThan(1001)
            .WithMessage("Amount of weight can be between 1 and 1000 kg's");

        When(x => exerciseType == ExerciseType.Cardio, () =>
        {
            RuleFor(x => x.DurationMinutes)
                .NotNull()
                .WithMessage("Duration is required for cardio exercises")
                .Must((model, minutes) => (minutes ?? 0) + (model.DurationSeconds ?? 0) > 0)
                .WithMessage("Total duration (minutes + seconds) must be greater than 0");

            RuleFor(x => x.DurationSeconds)
                .NotNull()
                .WithMessage("Duration is required for cardio exercises")
                .Must((model, seconds) => (model.DurationMinutes ?? 0) + (seconds ?? 0) > 0)
                .WithMessage("Total duration (minutes + seconds) must be greater than 0");

            RuleFor(x => x.Distance)
                .GreaterThan(0)
                .When(x => x.Distance.HasValue)
                .WithMessage("Distance must be greater than 0 km");
        });

        When(x => exerciseType == ExerciseType.WeightLifting, () =>
        {
            RuleFor(x => x.Distance)
                .Null()
                .WithMessage("Distance is not applicable for strength exercises");
        });
    }
}