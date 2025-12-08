using FluentValidation;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WorkoutTrackerApi.DTO.ExerciseEntry;
using WorkoutTrackerApi.Enums;

namespace WorkoutTrackerApi.Validators;

public class ExerciseEntryDtoValidator : AbstractValidator<ExerciseEntryDto>
{
    public ExerciseEntryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.ExerciseType)
            .IsInEnum();

        RuleFor(x => x.Duration)
            .NotNull()
            .When(x => x.ExerciseType == ExerciseType.Cardio)
            .WithMessage("Duration is required for cardio exercises")
            .Must(duration => duration > TimeSpan.Zero)
            .When(x => x.ExerciseType == ExerciseType.Cardio && x.Duration.HasValue)
            .WithMessage("Duration must be greater than 0");

        RuleFor(x => x.DistanceKm)
            .GreaterThan(0)
            .When(x => x.ExerciseType == ExerciseType.Cardio && x.DistanceKm.HasValue)
            .WithMessage("Distance must be greater than 0 km");

        RuleFor(x => x.DistanceKm)
            .Null()
            .When(x => x.ExerciseType == ExerciseType.WeightLifting)
            .WithMessage("Distance is not applicable for strength exercises");

        RuleFor(x => x.AvgHeartRate)
            .InclusiveBetween(30, 220)
            .When(x => x.AvgHeartRate.HasValue);

        RuleFor(x => x.CaloriesBurned)
            .GreaterThan(0)
            .When(x => x.CaloriesBurned.HasValue)
            .LessThanOrEqualTo(10000)
            .When(x => x.CaloriesBurned.HasValue);

        RuleForEach(x => x.Sets)
            .SetValidator(new SetEntryDtoValidator());

    }
}