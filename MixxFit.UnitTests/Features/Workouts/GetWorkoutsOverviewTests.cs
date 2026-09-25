using AwesomeAssertions;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Workouts.GetWorkoutsOverview;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Workouts.WorkoutTestData;

namespace MixxFit.UnitTests.Features.Workouts;

public class GetWorkoutsOverviewTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetWorkoutsOverviewHandler _handler;

    public GetWorkoutsOverviewTests()
    {
        _handler = new GetWorkoutsOverviewHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private void Seed()
    {
        _context.Workouts.AddRange(
            Workout(Utc(2025, 3, 5), "Push A", UserId, Entry(ExerciseType.WeightLifting, sets: 2)),
            Workout(Utc(2025, 11, 20), "Pull A", UserId, Entry(ExerciseType.WeightLifting), Entry(ExerciseType.Cardio)),
            Workout(Utc(2026, 1, 10), "Legs A", UserId, Entry(ExerciseType.Cardio)),
            Workout(Utc(2026, 2, 3), "Push B", UserId, Entry(ExerciseType.Cardio)),
            Workout(Utc(2026, 2, 25), "Push C", UserId, Entry(ExerciseType.BodyWeight)),
            Workout(Utc(2027, 1, 1), "Other user", OtherUserId, Entry(ExerciseType.Stretching), Entry(ExerciseType.Stretching)));
        _context.SaveChanges();
    }

    [Fact]
    public async Task Handle_WhenUserHasNoWorkouts_ShouldReturnEmptyOverview()
    {
        var result = await _handler.Handle(UserId, new GetWorkoutsOverviewRequest(), CancellationToken.None);

        result.Year.Should().BeNull();
        result.Month.Should().BeNull();
        result.AvailableYears.Should().BeEmpty();
        result.AvailableMonths.Should().BeEmpty();
        result.Workouts.Should().BeEmpty();
        result.WorkoutSummary.Should().Be(new WorkoutSummaryDto(0, 0, null, ExerciseType.Other));
    }

    [Fact]
    public async Task Handle_WithoutYearAndMonth_ShouldDefaultToLatestYearAndMonthWithDescendingLists()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsOverviewRequest(), CancellationToken.None);

        result.Year.Should().Be(2026);
        result.Month.Should().Be(2);
        result.AvailableYears.Should().Equal(2026, 2025);
        result.AvailableMonths.Should().Equal(2, 1);
        result.Workouts.Select(w => w.Name).Should().BeEquivalentTo("Push B", "Push C");
    }

    [Fact]
    public async Task Handle_WithYearOnly_ShouldDefaultToLatestMonthOfThatYear()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsOverviewRequest(Year: 2025), CancellationToken.None);

        result.Month.Should().Be(11);
        result.AvailableMonths.Should().Equal(11, 3);
        result.Workouts.Select(w => w.Name).Should().Equal("Pull A");
    }

    [Fact]
    public async Task Handle_WithYearAndMonth_ShouldReturnWorkoutsForThatMonth()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsOverviewRequest(Year: 2025, Month: Month.March), CancellationToken.None);

        result.Month.Should().Be(3);
        result.Workouts.Select(w => w.Name).Should().Equal("Push A");
    }

    [Fact]
    public async Task Handle_WithSearch_ShouldFilterByName()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsOverviewRequest(Search: "C"), CancellationToken.None);

        result.Workouts.Select(w => w.Name).Should().Equal("Push C");
    }

    [Theory]
    [InlineData("oldest", new[] { "Push B", "Push C" })]
    [InlineData("newest", new[] { "Push C", "Push B" })]
    public async Task Handle_WithSort_ShouldOrderWorkouts(string sort, string[] expected)
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsOverviewRequest(Sort: sort), CancellationToken.None);

        result.Workouts.Select(w => w.Name).Should().Equal(expected);
    }

    [Fact]
    public async Task Handle_ShouldProjectListItems()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsOverviewRequest(Year: 2025, Month: Month.November), CancellationToken.None);

        var item = result.Workouts.Should().ContainSingle().Subject;
        item.ExerciseCount.Should().Be(2);
        item.SetCount.Should().Be(2);
        item.HasWeights.Should().BeTrue();
        item.HasCardio.Should().BeTrue();
        item.HasBodyWeight.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldBuildSummaryAcrossAllOfTheUsersWorkouts()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutsOverviewRequest(Year: 2025, Month: Month.March), CancellationToken.None);

        result.WorkoutSummary.WorkoutCount.Should().Be(5);
        result.WorkoutSummary.ExerciseCount.Should().Be(6);
        result.WorkoutSummary.LastWorkoutDate.Should().Be(Utc(2026, 2, 25));
        result.WorkoutSummary.FavoriteExerciseType.Should().Be(ExerciseType.Cardio);
    }
}
