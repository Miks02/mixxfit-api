using FluentValidation.TestHelper;
using MixxFit.API.Features.Nutrition.SetDailyCalories;

namespace MixxFit.UnitTests.Features.Nutrition;

public class SetDailyCaloriesValidatorTests
{
    private readonly SetDailyCaloriesValidator _validator = new();

    [Fact]
    public void Validate_WhenRequestIsValid_ShouldNotHaveErrors()
    {
        _validator.TestValidate(new SetDailyCaloriesRequest(2200)).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenCaloriesAreZero_ShouldHaveRequiredError()
    {
        _validator.TestValidate(new SetDailyCaloriesRequest(0))
            .ShouldHaveValidationErrorFor(r => r.Calories)
            .WithErrorMessage("Calories are required");
    }

    [Theory]
    [InlineData(999.9)]
    [InlineData(10000.1)]
    [InlineData(-2000)]
    public void Validate_WhenCaloriesAreOutOfRange_ShouldHaveError(double calories)
    {
        _validator.TestValidate(new SetDailyCaloriesRequest(calories))
            .ShouldHaveValidationErrorFor(r => r.Calories)
            .WithErrorMessage("Calories must be between 1000 and 10000");
    }

    [Theory]
    [InlineData(1000)]
    [InlineData(10000)]
    public void Validate_WhenCaloriesAreOnBoundary_ShouldNotHaveError(double calories)
    {
        _validator.TestValidate(new SetDailyCaloriesRequest(calories))
            .ShouldNotHaveValidationErrorFor(r => r.Calories);
    }
}
