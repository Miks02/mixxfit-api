using FluentValidation.TestHelper;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Nutrition.CalculateCalories;

namespace MixxFit.UnitTests.Features.Nutrition;

public class CalculateCaloriesValidatorTests
{
    private readonly CalculateCaloriesValidator _validator = new();

    private static CalculateCaloriesRequest ValidRequest() => new()
    {
        Age = 30,
        Height = 180,
        Weight = 82,
        Gender = Gender.Male,
        ActivityLevel = ActivityLevel.Moderate
    };

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(ValidRequest()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenAgeIsZero_ShouldHaveRequiredError()
    {
        var request = ValidRequest() with { Age = 0 };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Age)
            .WithErrorMessage("Age is required");
    }

    [Theory]
    [InlineData(12)]
    [InlineData(131)]
    [InlineData(-1)]
    public void Validate_WhenAgeIsOutOfRange_ShouldHaveError(int age)
    {
        var request = ValidRequest() with { Age = age };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Age)
            .WithErrorMessage("Age must be between 13 and 130 years old.");
    }

    [Theory]
    [InlineData(13)]
    [InlineData(130)]
    public void Validate_WhenAgeIsOnBoundary_ShouldNotHaveError(int age)
    {
        var request = ValidRequest() with { Age = age };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Age);
    }

    [Fact]
    public void Validate_WhenGenderIsNotSet_ShouldHaveRequiredError()
    {
        var request = ValidRequest() with { Gender = default };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Gender)
            .WithErrorMessage("Gender is required");
    }

    [Fact]
    public void Validate_WhenGenderIsNotDefinedInEnum_ShouldHaveError()
    {
        var request = ValidRequest() with { Gender = (Gender)99 };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Gender)
            .WithErrorMessage("Invalid gender enum.");
    }

    [Theory]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    [InlineData(Gender.Other)]
    public void Validate_WhenGenderIsDefined_ShouldNotHaveError(Gender gender)
    {
        var request = ValidRequest() with { Gender = gender };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Gender);
    }

    [Fact]
    public void Validate_WhenHeightIsZero_ShouldHaveRequiredError()
    {
        var request = ValidRequest() with { Height = 0 };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Height)
            .WithErrorMessage("Height is required");
    }

    [Theory]
    [InlineData(69.9)]
    [InlineData(250.1)]
    [InlineData(-180)]
    public void Validate_WhenHeightIsOutOfRange_ShouldHaveError(double height)
    {
        var request = ValidRequest() with { Height = height };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Height)
            .WithErrorMessage("Height must be between 70 and 250 cm.");
    }

    [Theory]
    [InlineData(70)]
    [InlineData(250)]
    public void Validate_WhenHeightIsOnBoundary_ShouldNotHaveError(double height)
    {
        var request = ValidRequest() with { Height = height };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Height);
    }

    [Fact]
    public void Validate_WhenWeightIsZero_ShouldHaveRequiredError()
    {
        var request = ValidRequest() with { Weight = 0 };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Weight)
            .WithErrorMessage("Weight is required");
    }

    [Theory]
    [InlineData(24.9)]
    [InlineData(400.1)]
    [InlineData(-80)]
    public void Validate_WhenWeightIsOutOfRange_ShouldHaveError(double weight)
    {
        var request = ValidRequest() with { Weight = weight };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Weight)
            .WithErrorMessage("Weight must be between 25 and 400 kg.");
    }

    [Theory]
    [InlineData(25)]
    [InlineData(400)]
    public void Validate_WhenWeightIsOnBoundary_ShouldNotHaveError(double weight)
    {
        var request = ValidRequest() with { Weight = weight };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Weight);
    }

    [Fact]
    public void Validate_WhenActivityLevelIsNotDefinedInEnum_ShouldHaveError()
    {
        var request = ValidRequest() with { ActivityLevel = (ActivityLevel)99 };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.ActivityLevel)
            .WithErrorMessage("Invalid activity level enum.");
    }

    [Theory]
    [InlineData(ActivityLevel.Sedentary)]
    [InlineData(ActivityLevel.Light)]
    [InlineData(ActivityLevel.Moderate)]
    [InlineData(ActivityLevel.Active)]
    [InlineData(ActivityLevel.VeryActive)]
    public void Validate_WhenActivityLevelIsDefined_ShouldNotHaveError(ActivityLevel activityLevel)
    {
        var request = ValidRequest() with { ActivityLevel = activityLevel };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.ActivityLevel);
    }
}
