using FluentValidation.TestHelper;
using MixxFit.API.Features.WeightEntries.LogWeight;

namespace MixxFit.UnitTests.Features.WeightEntries;

public class LogWeightValidatorTests
{
    private readonly LogWeightValidator _validator = new();

    private static LogWeightRequest ValidRequest() => new()
    {
        Weight = 80m,
        Time = TimeSpan.FromHours(8),
        Notes = "Morning"
    };

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(ValidRequest()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenWeightIsZero_ShouldHaveRequiredError()
    {
        var request = ValidRequest() with { Weight = 0m };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Weight)
            .WithErrorMessage("Weight is required");
    }

    [Theory]
    [InlineData("25")]
    [InlineData("10")]
    [InlineData("-5")]
    public void Validate_WhenWeightIsNotAbove25_ShouldHaveError(string weight)
    {
        var request = ValidRequest() with { Weight = decimal.Parse(weight) };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Weight)
            .WithErrorMessage("Weight has to be higher than 25 KG");
    }

    [Theory]
    [InlineData("400")]
    [InlineData("500")]
    public void Validate_WhenWeightIsNotBelow400_ShouldHaveError(string weight)
    {
        var request = ValidRequest() with { Weight = decimal.Parse(weight) };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Weight)
            .WithErrorMessage("Weight has to be lower than 400 KG");
    }

    [Theory]
    [InlineData("25.01")]
    [InlineData("399.99")]
    public void Validate_WhenWeightIsWithinBounds_ShouldNotHaveError(string weight)
    {
        var request = ValidRequest() with { Weight = decimal.Parse(weight, System.Globalization.CultureInfo.InvariantCulture) };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Weight);
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
    public void Validate_WhenNotesAreExactly100Characters_ShouldNotHaveError()
    {
        var request = ValidRequest() with { Notes = new string('a', 100) };

        _validator.TestValidate(request).ShouldNotHaveValidationErrorFor(r => r.Notes);
    }

    [Fact]
    public void Validate_WhenNotesExceed100Characters_ShouldHaveError()
    {
        var request = ValidRequest() with { Notes = new string('a', 101) };

        _validator.TestValidate(request)
            .ShouldHaveValidationErrorFor(r => r.Notes)
            .WithErrorMessage("Maximum length is 100 characters");
    }
}
