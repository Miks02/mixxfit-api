using AwesomeAssertions;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Workouts.GetWorkouts;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Workouts.WorkoutTestData;

namespace MixxFit.UnitTests.Features.Workouts;

public class GetWorkoutsTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetWorkoutsHandler _handler;

    public GetWorkoutsTests()
    {
        _handler = new GetWorkoutsHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private void Seed()
    {
        _context.Workouts.AddRange(
            Workout(Utc(2025, 3, 5), "Push A"),
            Workout(Utc(2025, 11, 20), "Pull A"),
            Workout(Utc(2026, 1, 10), "Legs A"),
            Workout(Utc(2026, 2, 3), "Push B"),
            Workout(Utc(2026, 2, 17), "Pull B"),
            Workout(Utc(2026, 2, 25), "Push C"),
            Workout(Utc(2027, 1, 1), "Other user", OtherUserId));
        _context.SaveChanges();
    }

    [Fact]
    public async Task Handle_WhenUserHasNoWorkouts_ShouldReturnEmptyResponse()
    {
        var result = await _handler.Handle(UserId, new GetWorkoutsRequest(), CancellationToken.None);

        result.Year.Should().BeNull();
        result.Month.Should().BeNull();
        result.AvailableYears.Should().BeEmpty();
        result.AvailableMonths.Should().BeEmpty();
        result.Workouts.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithoutYearAndMonth_ShouldDefaultToLatestYearAndMonth()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest(), CancellationToken.None);

        result.Year.Should().Be(2026);
        result.Month.Should().Be(2);
        result.AvailableYears.Should().Equal(2025, 2026);
        result.AvailableMonths.Should().Equal(1, 2);
        result.Workouts.Select(w => w.Name).Should().BeEquivalentTo("Push B", "Pull B", "Push C");
    }

    [Fact]
    public async Task Handle_WithYearOnly_ShouldDefaultToLatestMonthOfThatYear()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest { Year = 2025 }, CancellationToken.None);

        result.Year.Should().Be(2025);
        result.Month.Should().Be(11);
        result.AvailableMonths.Should().Equal(3, 11);
        result.Workouts.Select(w => w.Name).Should().Equal("Pull A");
    }

    [Fact]
    public async Task Handle_WithYearAndMonth_ShouldReturnWorkoutsForThatMonth()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest { Year = 2025, Month = Month.March }, CancellationToken.None);

        result.Year.Should().Be(2025);
        result.Month.Should().Be(3);
        result.Workouts.Select(w => w.Name).Should().Equal("Push A");
    }

    [Fact]
    public async Task Handle_WhenMonthHasNoWorkouts_ShouldReturnEmptyWorkoutList()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest { Year = 2026, Month = Month.July }, CancellationToken.None);

        result.Month.Should().Be(7);
        result.AvailableMonths.Should().Equal(1, 2);
        result.Workouts.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenYearHasNoWorkouts_ShouldReturnNoMonthsAndNoWorkouts()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest { Year = 2020 }, CancellationToken.None);

        result.Year.Should().Be(2020);
        result.Month.Should().BeNull();
        result.AvailableMonths.Should().BeEmpty();
        result.Workouts.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldNotReturnOtherUsersData()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest { Year = 2027 }, CancellationToken.None);

        result.AvailableYears.Should().NotContain(2027);
        result.Workouts.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithSearch_ShouldFilterByName()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest { Search = "Push" }, CancellationToken.None);

        result.Workouts.Select(w => w.Name).Should().BeEquivalentTo("Push B", "Push C");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_WhenSearchIsBlank_ShouldIgnoreSearch(string search)
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest { Search = search }, CancellationToken.None);

        result.Workouts.Should().HaveCount(3);
    }

    [Theory]
    [InlineData("oldest", new[] { "Push B", "Pull B", "Push C" })]
    [InlineData("newest", new[] { "Push C", "Pull B", "Push B" })]
    [InlineData("unknown", new[] { "Push C", "Pull B", "Push B" })]
    public async Task Handle_WithSort_ShouldOrderWorkouts(string sort, string[] expected)
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest { Sort = sort }, CancellationToken.None);

        result.Workouts.Select(w => w.Name).Should().Equal(expected);
    }

    [Fact]
    public async Task Handle_ShouldProjectExerciseAndSetCountsAndTypeFlags()
    {
        _context.Workouts.AddRange(
            Workout(Utc(2026, 4, 1), "Mixed", UserId,
                Entry(ExerciseType.WeightLifting, sets: 3),
                Entry(ExerciseType.Cardio, sets: 1),
                Entry(ExerciseType.BodyWeight, sets: 2)),
            Workout(Utc(2026, 4, 2), "Only weights", UserId,
                Entry(ExerciseType.WeightLifting, sets: 4)));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, new GetWorkoutsRequest(), CancellationToken.None);

        var mixed = result.Workouts.Single(w => w.Name == "Mixed");
        mixed.ExerciseCount.Should().Be(3);
        mixed.SetCount.Should().Be(6);
        mixed.HasWeights.Should().BeTrue();
        mixed.HasCardio.Should().BeTrue();
        mixed.HasBodyWeight.Should().BeTrue();
        mixed.WorkoutDate.Should().Be(Utc(2026, 4, 1));

        var weights = result.Workouts.Single(w => w.Name == "Only weights");
        weights.ExerciseCount.Should().Be(1);
        weights.SetCount.Should().Be(4);
        weights.HasWeights.Should().BeTrue();
        weights.HasCardio.Should().BeFalse();
        weights.HasBodyWeight.Should().BeFalse();
    }
}
