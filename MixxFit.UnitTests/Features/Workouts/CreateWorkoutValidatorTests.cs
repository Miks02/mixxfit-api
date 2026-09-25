using AwesomeAssertions;
using FluentValidation.TestHelper;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Workouts.CreateWorkout;

namespace MixxFit.UnitTests.Features.Workouts;

public class CreateWorkoutValidatorTests
{
    private readonly CreateWorkoutValidator _validator = new();

    private static ExerciseEntryDto ValidEntry() => new()
    {
        ExerciseId = 1,
        Name = "Bench Press",
        ExerciseType = ExerciseType.WeightLifting,
        Sets = [new SetEntryDto { Reps = 10, Weight = 60m }]
    };

    private static CreateWorkoutRequest ValidRequest() => new()
    {
        Name = "Push Day",
        Notes = "Notes",
        WorkoutDate = DateTime.UtcNow.AddDays(-1),
        ExerciseEntries = [ValidEntry()]
    };

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(ValidRequest()).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WhenNameIsEmpty_ShouldHaveError(string? name)
    {
        var request = ValidRequest() with { Name = name! };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Fact]
    public void Validate_WhenNameExceeds100Characters_ShouldHaveError()
    {
        var request = ValidRequest() with { Name = new string('a', 101) };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Name is too long (100 characters max)");
    }

    [Fact]
    public void Validate_WhenNameIsExactly100Characters_ShouldNotHaveError()
    {
        var request = ValidRequest() with { Name = new string('a', 100) };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Name);
    }

    [Fact]
    public void Validate_WhenWorkoutDateIsDefault_ShouldHaveError()
    {
        var request = ValidRequest() with { WorkoutDate = default };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.WorkoutDate);
    }

    [Fact]
    public void Validate_WhenWorkoutDateIsInFuture_ShouldHaveError()
    {
        var request = ValidRequest() with { WorkoutDate = DateTime.UtcNow.AddDays(1) };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.WorkoutDate)
            .WithErrorMessage("Workout date cannot be in the future");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WhenNotesAreEmpty_ShouldNotHaveError(string? notes)
    {
        var request = ValidRequest() with { Notes = notes };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Notes);
    }

    [Fact]
    public void Validate_WhenNotesExceed150Characters_ShouldHaveError()
    {
        var request = ValidRequest() with { Notes = new string('a', 151) };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.Notes);
    }

    [Fact]
    public void Validate_WhenExerciseEntriesAreEmpty_ShouldHaveError()
    {
        var request = ValidRequest() with { ExerciseEntries = [] };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.ExerciseEntries);
    }

    [Fact]
    public void Validate_WhenMoreThan100ExerciseEntries_ShouldHaveError()
    {
        var request = ValidRequest() with { ExerciseEntries = Enumerable.Range(0, 101).Select(_ => ValidEntry()).ToList() };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.ExerciseEntries)
            .WithErrorMessage("Maximum of 100 exercise entries are allowed");
    }

    [Fact]
    public void Validate_WhenExactly100ExerciseEntries_ShouldNotHaveError()
    {
        var request = ValidRequest() with { ExerciseEntries = Enumerable.Range(0, 100).Select(_ => ValidEntry()).ToList() };

        _validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenNestedExerciseEntryIsInvalid_ShouldHaveNestedError()
    {
        var request = ValidRequest() with { ExerciseEntries = [ValidEntry() with { Name = "" }] };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor("ExerciseEntries[0].Name");
    }

    [Fact]
    public void Validate_WhenNestedSetIsInvalid_ShouldHaveNestedError()
    {
        var request = ValidRequest() with
        {
            ExerciseEntries = [ValidEntry() with { Sets = [new SetEntryDto { Reps = 0 }] }]
        };

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor("ExerciseEntries[0].Sets[0].Reps");
    }
}

public class ExerciseEntryValidatorTests
{
    private readonly ExerciseEntryValidator _validator = new();

    private static ExerciseEntryDto ValidEntry() => new()
    {
        ExerciseId = 1,
        Name = "Bench Press",
        ExerciseType = ExerciseType.WeightLifting,
        Sets = [new SetEntryDto { Reps = 10, Weight = 60m }]
    };

    [Fact]
    public void Validate_WhenEntryIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(ValidEntry()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldHaveError()
    {
        _validator.TestValidate(ValidEntry() with { Name = "" }).ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Fact]
    public void Validate_WhenNameExceeds100Characters_ShouldHaveError()
    {
        _validator.TestValidate(ValidEntry() with { Name = new string('a', 101) }).ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Fact]
    public void Validate_WhenExerciseIdIsZero_ShouldHaveError()
    {
        _validator.TestValidate(ValidEntry() with { ExerciseId = 0 }).ShouldHaveValidationErrorFor(e => e.ExerciseId);
    }

    [Fact]
    public void Validate_WhenExerciseTypeIsNotDefined_ShouldHaveError()
    {
        _validator.TestValidate(ValidEntry() with { ExerciseType = (ExerciseType)99 }).ShouldHaveValidationErrorFor(e => e.ExerciseType);
    }

    [Fact]
    public void Validate_WhenSetsAreEmpty_ShouldHaveError()
    {
        _validator.TestValidate(ValidEntry() with { Sets = [] }).ShouldHaveValidationErrorFor(e => e.Sets);
    }

    [Fact]
    public void Validate_WhenCardioSetHasNoDuration_ShouldUseCardioRulesForSets()
    {
        var entry = ValidEntry() with { ExerciseType = ExerciseType.Cardio, Sets = [new SetEntryDto { Distance = 3m }] };

        _validator.TestValidate(entry).ShouldHaveValidationErrorFor("Sets[0].DurationMinutes");
    }
}

public class SetEntryValidatorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1001)]
    public void Validate_WhenRepsAreOutOfRange_ShouldHaveError(int reps)
    {
        var validator = new SetEntryValidator(ExerciseType.WeightLifting);

        validator.TestValidate(new SetEntryDto { Reps = reps }).ShouldHaveValidationErrorFor(s => s.Reps);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1000)]
    public void Validate_WhenRepsAreInRange_ShouldNotHaveError(int reps)
    {
        var validator = new SetEntryValidator(ExerciseType.WeightLifting);

        validator.TestValidate(new SetEntryDto { Reps = reps }).ShouldNotHaveValidationErrorFor(s => s.Reps);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-5")]
    [InlineData("1001")]
    public void Validate_WhenWeightIsOutOfRange_ShouldHaveError(string weight)
    {
        var validator = new SetEntryValidator(ExerciseType.WeightLifting);

        validator.TestValidate(new SetEntryDto { Weight = decimal.Parse(weight) }).ShouldHaveValidationErrorFor(s => s.Weight);
    }

    [Fact]
    public void Validate_WhenRepsAndWeightAreNull_ShouldNotHaveErrors()
    {
        var validator = new SetEntryValidator(ExerciseType.BodyWeight);

        validator.TestValidate(new SetEntryDto()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenWeightLiftingSetHasDistance_ShouldHaveError()
    {
        var validator = new SetEntryValidator(ExerciseType.WeightLifting);

        validator.TestValidate(new SetEntryDto { Reps = 5, Weight = 100m, Distance = 1m })
            .ShouldHaveValidationErrorFor(s => s.Distance)
            .WithErrorMessage("Distance is not applicable for strength exercises");
    }

    [Fact]
    public void Validate_WhenCardioSetIsMissingDuration_ShouldHaveErrors()
    {
        var validator = new SetEntryValidator(ExerciseType.Cardio);

        var result = validator.TestValidate(new SetEntryDto { Distance = 5m });

        result.ShouldHaveValidationErrorFor(s => s.DurationMinutes);
        result.ShouldHaveValidationErrorFor(s => s.DurationSeconds);
    }

    [Fact]
    public void Validate_WhenCardioTotalDurationIsZero_ShouldHaveErrors()
    {
        var validator = new SetEntryValidator(ExerciseType.Cardio);

        var result = validator.TestValidate(new SetEntryDto { DurationMinutes = 0, DurationSeconds = 0 });

        result.ShouldHaveValidationErrorFor(s => s.DurationMinutes)
            .WithErrorMessage("Total duration (minutes + seconds) must be greater than 0");
        result.ShouldHaveValidationErrorFor(s => s.DurationSeconds);
    }

    [Theory]
    [InlineData(0, 30)]
    [InlineData(20, 0)]
    [InlineData(20, 15)]
    public void Validate_WhenCardioHasPositiveTotalDuration_ShouldNotHaveErrors(int minutes, int seconds)
    {
        var validator = new SetEntryValidator(ExerciseType.Cardio);

        validator.TestValidate(new SetEntryDto { DurationMinutes = minutes, DurationSeconds = seconds, Distance = 5m })
            .ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenCardioDistanceIsNotPositive_ShouldHaveError()
    {
        var validator = new SetEntryValidator(ExerciseType.Cardio);

        validator.TestValidate(new SetEntryDto { DurationMinutes = 10, DurationSeconds = 0, Distance = 0m })
            .ShouldHaveValidationErrorFor(s => s.Distance);
    }

    [Theory]
    [InlineData(ExerciseType.BodyWeight)]
    [InlineData(ExerciseType.Stretching)]
    [InlineData(ExerciseType.Other)]
    public void Validate_WhenNonCardioNonWeightLiftingSetHasNoDuration_ShouldNotHaveErrors(ExerciseType type)
    {
        var validator = new SetEntryValidator(type);

        validator.TestValidate(new SetEntryDto { Reps = 10, Distance = 1m }).ShouldNotHaveAnyValidationErrors();
    }
}
