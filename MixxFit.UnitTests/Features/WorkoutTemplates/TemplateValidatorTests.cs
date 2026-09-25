using FluentValidation.TestHelper;
using MixxFit.API.Features.WorkoutTemplates.CreateTemplate;
using MixxFit.API.Features.WorkoutTemplates.EditTemplate;
using CreateExerciseItemValidator = MixxFit.API.Features.WorkoutTemplates.CreateTemplate.ExerciseItemValidator;
using EditExerciseItemValidator = MixxFit.API.Features.WorkoutTemplates.EditTemplate.ExerciseItemValidator;

namespace MixxFit.UnitTests.Features.WorkoutTemplates;

public class CreateTemplateValidatorTests
{
    private readonly CreateTemplateValidator _validator = new();

    private static CreateTemplateRequest.ExerciseItem Item(int exerciseId = 1, int setCount = 3) => new() { ExerciseId = exerciseId, SetCount = setCount };

    private static CreateTemplateRequest ValidRequest() => new()
    {
        Name = "Push Day",
        Notes = "Chest focus",
        Exercises = [Item()]
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
        _validator.TestValidate(ValidRequest() with { Name = name! })
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Template name cannot be empty");
    }

    [Fact]
    public void Validate_WhenNameIsShorterThan4Characters_ShouldHaveError()
    {
        _validator.TestValidate(ValidRequest() with { Name = "abc" }).ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Fact]
    public void Validate_WhenNameExceeds100Characters_ShouldHaveError()
    {
        _validator.TestValidate(ValidRequest() with { Name = new string('a', 101) })
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Template name must be between 4 and 100 characters");
    }

    [Theory]
    [InlineData(4)]
    [InlineData(100)]
    public void Validate_WhenNameLengthIsOnBoundary_ShouldNotHaveError(int length)
    {
        _validator.TestValidate(ValidRequest() with { Name = new string('a', length) }).ShouldNotHaveValidationErrorFor(r => r.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WhenNotesAreEmpty_ShouldNotHaveError(string? notes)
    {
        _validator.TestValidate(ValidRequest() with { Notes = notes }).ShouldNotHaveValidationErrorFor(r => r.Notes);
    }

    [Fact]
    public void Validate_WhenNotesExceed200Characters_ShouldHaveError()
    {
        _validator.TestValidate(ValidRequest() with { Notes = new string('a', 201) })
            .ShouldHaveValidationErrorFor(r => r.Notes)
            .WithErrorMessage("Notes cannot exceed 200 characters");
    }

    [Fact]
    public void Validate_WhenExercisesAreEmpty_ShouldHaveError()
    {
        _validator.TestValidate(ValidRequest() with { Exercises = [] })
            .ShouldHaveValidationErrorFor(r => r.Exercises)
            .WithErrorMessage("Template must contain at least one exercise");
    }

    [Fact]
    public void Validate_WhenExercisesAreNull_ShouldHaveErrorInsteadOfThrowing()
    {
        _validator.TestValidate(ValidRequest() with { Exercises = null! })
            .ShouldHaveValidationErrorFor(r => r.Exercises)
            .WithErrorMessage("Template must contain at least one exercise");
    }

    [Fact]
    public void Validate_WhenMoreThan50Exercises_ShouldHaveError()
    {
        var request = ValidRequest() with { Exercises = Enumerable.Range(1, 51).Select(i => Item(i)).ToList() };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Exercises)
            .WithErrorMessage("Template cannot contain more than 50 exercises");
    }

    [Fact]
    public void Validate_WhenExactly50Exercises_ShouldNotHaveError()
    {
        var request = ValidRequest() with { Exercises = Enumerable.Range(1, 50).Select(i => Item(i)).ToList() };

        _validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenSameExerciseIsUsedTwice_ShouldHaveError()
    {
        var request = ValidRequest() with { Exercises = [Item(1, 3), Item(2, 3), Item(1, 5)] };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Exercises)
            .WithErrorMessage("Template cannot contain the same exercise more than once");
    }

    [Fact]
    public void Validate_WhenExercisesAreDistinct_ShouldNotHaveError()
    {
        var request = ValidRequest() with { Exercises = [Item(1), Item(2), Item(3)] };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Exercises);
    }

    [Fact]
    public void Validate_WhenNestedExerciseIsInvalid_ShouldHaveNestedError()
    {
        var request = ValidRequest() with { Exercises = [Item(), Item(exerciseId: 2, setCount: 0)] };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor("Exercises[1].SetCount");
    }
}

public class CreateTemplateExerciseItemValidatorTests
{
    private readonly CreateExerciseItemValidator _validator = new();

    [Fact]
    public void Validate_WhenItemIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(new CreateTemplateRequest.ExerciseItem { ExerciseId = 1, SetCount = 3 }).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExerciseIdIsZero_ShouldHaveError()
    {
        _validator.TestValidate(new CreateTemplateRequest.ExerciseItem { ExerciseId = 0, SetCount = 3 })
            .ShouldHaveValidationErrorFor(e => e.ExerciseId)
            .WithErrorMessage("Exercise is required");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenSetCountIsNotPositive_ShouldHaveError(int setCount)
    {
        _validator.TestValidate(new CreateTemplateRequest.ExerciseItem { ExerciseId = 1, SetCount = setCount })
            .ShouldHaveValidationErrorFor(e => e.SetCount)
            .WithErrorMessage("Set count must be greater than 0");
    }

    [Fact]
    public void Validate_WhenSetCountExceeds50_ShouldHaveError()
    {
        _validator.TestValidate(new CreateTemplateRequest.ExerciseItem { ExerciseId = 1, SetCount = 51 })
            .ShouldHaveValidationErrorFor(e => e.SetCount)
            .WithErrorMessage("Set count must be less than or equal to 50");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    public void Validate_WhenSetCountIsOnBoundary_ShouldNotHaveError(int setCount)
    {
        _validator.TestValidate(new CreateTemplateRequest.ExerciseItem { ExerciseId = 1, SetCount = setCount })
            .ShouldNotHaveValidationErrorFor(e => e.SetCount);
    }
}

public class EditTemplateValidatorTests
{
    private readonly EditTemplateValidator _validator = new();

    private static EditTemplateRequest.ExerciseItem Item(int exerciseId = 1, int setCount = 3) => new() { ExerciseId = exerciseId, SetCount = setCount };

    private static EditTemplateRequest ValidRequest() => new()
    {
        Id = 1,
        Name = "Push Day",
        Notes = "Chest focus",
        Exercises = [Item()]
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
        _validator.TestValidate(ValidRequest() with { Name = name! })
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Template name cannot be empty");
    }

    [Fact]
    public void Validate_WhenNameIsShorterThan4Characters_ShouldHaveError()
    {
        _validator.TestValidate(ValidRequest() with { Name = "abc" }).ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Fact]
    public void Validate_WhenNameExceeds100Characters_ShouldHaveError()
    {
        _validator.TestValidate(ValidRequest() with { Name = new string('a', 101) }).ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Fact]
    public void Validate_WhenNotesExceed200Characters_ShouldHaveError()
    {
        _validator.TestValidate(ValidRequest() with { Notes = new string('a', 201) }).ShouldHaveValidationErrorFor(r => r.Notes);
    }

    [Fact]
    public void Validate_WhenExercisesAreEmpty_ShouldHaveError()
    {
        _validator.TestValidate(ValidRequest() with { Exercises = [] }).ShouldHaveValidationErrorFor(r => r.Exercises);
    }

    [Fact]
    public void Validate_WhenExercisesAreNull_ShouldHaveErrorInsteadOfThrowing()
    {
        _validator.TestValidate(ValidRequest() with { Exercises = null! })
            .ShouldHaveValidationErrorFor(r => r.Exercises)
            .WithErrorMessage("Template must contain at least one exercise");
    }

    [Fact]
    public void Validate_WhenMoreThan50Exercises_ShouldHaveError()
    {
        var request = ValidRequest() with { Exercises = Enumerable.Range(1, 51).Select(i => Item(i)).ToList() };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor(r => r.Exercises);
    }

    [Fact]
    public void Validate_WhenSameExerciseIsUsedTwice_ShouldHaveError()
    {
        var request = ValidRequest() with { Exercises = [Item(1, 3), Item(1, 5)] };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Exercises)
            .WithErrorMessage("Template cannot contain the same exercise more than once");
    }

    [Fact]
    public void Validate_WhenNestedExerciseIsInvalid_ShouldHaveNestedError()
    {
        var request = ValidRequest() with { Exercises = [Item(exerciseId: 0)] };

        _validator.TestValidate(request).ShouldHaveValidationErrorFor("Exercises[0].ExerciseId");
    }
}

public class EditTemplateExerciseItemValidatorTests
{
    private readonly EditExerciseItemValidator _validator = new();

    [Fact]
    public void Validate_WhenItemIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(new EditTemplateRequest.ExerciseItem { ExerciseId = 1, SetCount = 3 }).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenExerciseIdIsZero_ShouldHaveError()
    {
        _validator.TestValidate(new EditTemplateRequest.ExerciseItem { ExerciseId = 0, SetCount = 3 })
            .ShouldHaveValidationErrorFor(e => e.ExerciseId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Validate_WhenSetCountIsOutOfRange_ShouldHaveError(int setCount)
    {
        _validator.TestValidate(new EditTemplateRequest.ExerciseItem { ExerciseId = 1, SetCount = setCount })
            .ShouldHaveValidationErrorFor(e => e.SetCount);
    }
}
