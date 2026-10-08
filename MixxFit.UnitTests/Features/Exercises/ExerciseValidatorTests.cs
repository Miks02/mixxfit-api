using FluentValidation.TestHelper;
using MixxFit.API.Features.Exercises.CreateExercise;
using MixxFit.API.Features.Exercises.UpdateExercise;

namespace MixxFit.UnitTests.Features.Exercises;

public class CreateExerciseValidatorTests
{
    private readonly CreateExerciseValidator _validator = new();

    private static CreateExerciseRequest Valid() => new() { Name = "Paused Bench", CategoryId = 2, MuscleGroupId = 1 };

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(Valid()).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Validate_WhenNameLengthIsInvalid_ShouldHaveError(string name)
    {
        _validator.TestValidate(Valid() with { Name = name }).ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Theory]
    [InlineData("abcd")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Validate_WhenNameLengthIsOnBoundary_ShouldNotHaveError(string name)
    {
        _validator.TestValidate(Valid() with { Name = name }).ShouldNotHaveValidationErrorFor(r => r.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenCategoryIdIsNotPositive_ShouldHaveError(int categoryId)
    {
        _validator.TestValidate(Valid() with { CategoryId = categoryId }).ShouldHaveValidationErrorFor(r => r.CategoryId);
    }

    [Fact]
    public void Validate_WhenCategoryIsOther_ShouldHaveError()
    {
        _validator.TestValidate(Valid() with { CategoryId = 5 }).ShouldHaveValidationErrorFor(r => r.CategoryId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenMuscleGroupIdIsNotPositive_ShouldHaveError(int muscleGroupId)
    {
        _validator.TestValidate(Valid() with { MuscleGroupId = muscleGroupId }).ShouldHaveValidationErrorFor(r => r.MuscleGroupId);
    }
}

public class UpdateExerciseValidatorTests
{
    private readonly UpdateExerciseValidator _validator = new();

    private static UpdateExerciseRequest Valid() => new() { Id = 1, Name = "Paused Bench", CategoryId = 2, MuscleGroupId = 1 };

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(Valid()).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Validate_WhenNameLengthIsInvalid_ShouldHaveError(string name)
    {
        _validator.TestValidate(Valid() with { Name = name }).ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void Validate_WhenCategoryIdIsInvalid_ShouldHaveError(int categoryId)
    {
        _validator.TestValidate(Valid() with { CategoryId = categoryId }).ShouldHaveValidationErrorFor(r => r.CategoryId);
    }

    [Fact]
    public void Validate_WhenMuscleGroupIdIsNotPositive_ShouldHaveError()
    {
        _validator.TestValidate(Valid() with { MuscleGroupId = 0 }).ShouldHaveValidationErrorFor(r => r.MuscleGroupId);
    }
}
