using AwesomeAssertions;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Nutrition.CalculateCalories;

namespace MixxFit.UnitTests.Features.Nutrition;

public class CalculateCaloriesTests
{
    private readonly CalculateCaloriesHandler _handler = new();

    private static CalculateCaloriesRequest Request(
        Gender gender = Gender.Male,
        ActivityLevel activityLevel = ActivityLevel.Moderate) => new()
    {
        Age = 30,
        Height = 180,
        Weight = 82,
        Gender = gender,
        ActivityLevel = activityLevel
    };

    [Theory]
    [InlineData(Gender.Male, 1800)]
    [InlineData(Gender.Female, 1634)]
    [InlineData(Gender.Other, 1717)]
    public void Handle_WhenGenderIsGiven_ShouldApplyGenderAdjustmentToBmr(Gender gender, double expectedBmr)
    {
        var response = _handler.Handle(Request(gender));

        response.Bmr.Should().Be(expectedBmr);
    }

    [Theory]
    [InlineData(ActivityLevel.Sedentary, 2160)]
    [InlineData(ActivityLevel.Light, 2475)]
    [InlineData(ActivityLevel.Moderate, 2790)]
    [InlineData(ActivityLevel.Active, 3105)]
    [InlineData(ActivityLevel.VeryActive, 3420)]
    public void Handle_WhenActivityLevelIsGiven_ShouldMultiplyBmrByActivityFactor(
        ActivityLevel activityLevel,
        double expectedMaintenance)
    {
        var response = _handler.Handle(Request(activityLevel: activityLevel));

        response.Maintenance.Should().Be(expectedMaintenance);
    }

    [Fact]
    public void Handle_WhenActivityLevelChanges_ShouldNotChangeBmr()
    {
        var sedentary = _handler.Handle(Request(activityLevel: ActivityLevel.Sedentary));
        var veryActive = _handler.Handle(Request(activityLevel: ActivityLevel.VeryActive));

        sedentary.Bmr.Should().Be(veryActive.Bmr);
    }

    [Fact]
    public void Handle_WhenCalled_ShouldReturnGoalsRelativeToMaintenance()
    {
        var response = _handler.Handle(Request());

        response.Should().BeEquivalentTo(new CalculateCaloriesResponse
        {
            Bmr = 1800,
            AggressiveLoss = 2290,
            MildLoss = 2540,
            Maintenance = 2790,
            MildGain = 3040,
            AggressiveGain = 3290
        });
    }

    [Fact]
    public void Handle_WhenResultHasDecimals_ShouldRoundBmrAndMaintenance()
    {
        var response = _handler.Handle(Request() with { Height = 175 });

        response.Bmr.Should().Be(1769);
        response.Maintenance.Should().Be(2742);
    }

    [Fact]
    public void Handle_WhenWeightIsHigher_ShouldReturnHigherBmr()
    {
        var lighter = _handler.Handle(Request() with { Weight = 70 });
        var heavier = _handler.Handle(Request() with { Weight = 90 });

        heavier.Bmr.Should().Be(lighter.Bmr + 200);
    }

    [Fact]
    public void Handle_WhenAgeIsHigher_ShouldReturnLowerBmr()
    {
        var younger = _handler.Handle(Request() with { Age = 20 });
        var older = _handler.Handle(Request() with { Age = 40 });

        older.Bmr.Should().Be(younger.Bmr - 100);
    }
}
