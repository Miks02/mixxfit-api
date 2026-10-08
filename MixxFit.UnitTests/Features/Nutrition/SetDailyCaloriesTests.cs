using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Nutrition.SetDailyCalories;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;

namespace MixxFit.UnitTests.Features.Nutrition;

public class SetDailyCaloriesTests : IDisposable
{
    private const string UserId = "user-1";
    private const string OtherUserId = "user-2";

    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly SetDailyCaloriesHandler _handler;

    public SetDailyCaloriesTests()
    {
        _context.Users.AddRange(
            UserWithProfile(UserId, dailyCalorieGoal: null),
            UserWithProfile(OtherUserId, dailyCalorieGoal: 1800));
        _context.SaveChanges();

        _handler = new SetDailyCaloriesHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private static User UserWithProfile(string userId, double? dailyCalorieGoal) => new()
    {
        Id = userId,
        UserName = userId,
        Email = $"{userId}@mail.com",
        FitnessProfile = new FitnessProfile { UserId = userId, DailyCalorieGoal = dailyCalorieGoal }
    };

    [Fact]
    public async Task Handle_WhenFitnessProfileExists_ShouldSetDailyCalorieGoalAndReturnIt()
    {
        var result = await _handler.Handle(UserId, new SetDailyCaloriesRequest(2200), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload.Should().Be(new SetDailyCaloriesResponse(2200));

        var profile = await _context.FitnessProfiles.AsNoTracking().SingleAsync(fp => fp.UserId == UserId);
        profile.DailyCalorieGoal.Should().Be(2200);
    }

    [Fact]
    public async Task Handle_WhenGoalAlreadyExists_ShouldOverwriteIt()
    {
        var result = await _handler.Handle(OtherUserId, new SetDailyCaloriesRequest(2500), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Calories.Should().Be(2500);

        var profile = await _context.FitnessProfiles.AsNoTracking().SingleAsync(fp => fp.UserId == OtherUserId);
        profile.DailyCalorieGoal.Should().Be(2500);
    }

    [Fact]
    public async Task Handle_WhenGoalIsSet_ShouldNotChangeOtherUsersGoal()
    {
        await _handler.Handle(UserId, new SetDailyCaloriesRequest(2200), CancellationToken.None);

        var otherProfile = await _context.FitnessProfiles.AsNoTracking().SingleAsync(fp => fp.UserId == OtherUserId);
        otherProfile.DailyCalorieGoal.Should().Be(1800);
    }

    [Fact]
    public async Task Handle_WhenFitnessProfileDoesNotExist_ShouldReturnNotFoundAndNotChangeAnything()
    {
        var result = await _handler.Handle("missing-user", new SetDailyCaloriesRequest(2200), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(UserError.NotFound("missing-user"));

        var goals = await _context.FitnessProfiles.AsNoTracking().Select(fp => fp.DailyCalorieGoal).ToListAsync();
        goals.Should().BeEquivalentTo(new double?[] { null, 1800 });
    }
}
