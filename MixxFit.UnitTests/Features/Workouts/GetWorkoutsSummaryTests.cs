using AwesomeAssertions;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Workouts.GetWorkoutsSummary;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Workouts.WorkoutTestData;

namespace MixxFit.UnitTests.Features.Workouts;

public class GetWorkoutsSummaryTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetWorkoutsSummaryHandler _handler;

    public GetWorkoutsSummaryTests()
    {
        _handler = new GetWorkoutsSummaryHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private static DateTime DaysAgo(int days) => DateTime.UtcNow.Date.AddDays(-days);

    private async Task SeedDaysAgoAsync(params int[] daysAgo)
    {
        _context.Workouts.AddRange(daysAgo.Select(d => Workout(DaysAgo(d))));
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ShouldReturnCountsLastDateAndFavoriteType()
    {
        _context.Workouts.AddRange(
            Workout(Utc(2026, 1, 10), "A", UserId, Entry(ExerciseType.WeightLifting), Entry(ExerciseType.BodyWeight)),
            Workout(Utc(2026, 2, 20), "B", UserId, Entry(ExerciseType.BodyWeight), Entry(ExerciseType.BodyWeight)),
            Workout(Utc(2026, 3, 5), "C", UserId, Entry(ExerciseType.Cardio)),
            Workout(Utc(2026, 9, 1), "Other", OtherUserId, Entry(ExerciseType.Cardio), Entry(ExerciseType.Cardio), Entry(ExerciseType.Cardio)));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.WorkoutCount.Should().Be(3);
        result.ExerciseCount.Should().Be(5);
        result.LastWorkoutDate.Should().Be(new DateOnly(2026, 3, 5));
        result.FavoriteExerciseType.Should().Be(ExerciseType.BodyWeight);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoWorkouts_ShouldReturnEmptySummary()
    {
        _context.Workouts.Add(Workout(DaysAgo(0), ownerId: OtherUserId));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.WorkoutCount.Should().Be(0);
        result.ExerciseCount.Should().Be(0);
        result.LastWorkoutDate.Should().BeNull();
        result.FavoriteExerciseType.Should().Be(ExerciseType.Other);
        result.WorkoutStreak.Should().Be(0);
        result.MostActiveMonths.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnTopFourMostActiveMonthsOrderedByCount()
    {
        var dates = new[]
        {
            (2025, 1, 4), (2025, 3, 1), (2025, 5, 3), (2025, 7, 2), (2026, 1, 1)
        };
        foreach (var (year, month, count) in dates)
            for (var day = 1; day <= count; day++)
                _context.Workouts.Add(Workout(Utc(year, month, day)));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.MostActiveMonths.Should().HaveCount(4);
        result.MostActiveMonths.Select(m => m.WorkoutCount).Should().BeInDescendingOrder();
        result.MostActiveMonths[0].Should().Be(new MostActiveMonthDto { Month = Month.January, Year = 2025, WorkoutCount = 4 });
        result.MostActiveMonths[1].Should().Be(new MostActiveMonthDto { Month = Month.May, Year = 2025, WorkoutCount = 3 });
        result.MostActiveMonths[2].Should().Be(new MostActiveMonthDto { Month = Month.July, Year = 2025, WorkoutCount = 2 });
        result.MostActiveMonths[3].WorkoutCount.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldGroupMostActiveMonthsByYearAndMonth()
    {
        _context.Workouts.AddRange(
            Workout(Utc(2025, 4, 1)),
            Workout(Utc(2026, 4, 1)),
            Workout(Utc(2026, 4, 2)));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.MostActiveMonths.Should().BeEquivalentTo(new[]
        {
            new MostActiveMonthDto { Month = Month.April, Year = 2026, WorkoutCount = 2 },
            new MostActiveMonthDto { Month = Month.April, Year = 2025, WorkoutCount = 1 }
        }, o => o.WithStrictOrdering());
    }

    [Fact]
    public async Task Handle_WhenWorkoutsOnConsecutiveDaysIncludingToday_ShouldCountStreak()
    {
        await SeedDaysAgoAsync(0, 1, 2, 5);

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.WorkoutStreak.Should().Be(3);
    }

    [Fact]
    public async Task Handle_WhenLastWorkoutWasYesterday_ShouldKeepStreakAlive()
    {
        await SeedDaysAgoAsync(1, 2);

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.WorkoutStreak.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WhenLastWorkoutWasTwoOrMoreDaysAgo_ShouldReturnZeroStreak()
    {
        await SeedDaysAgoAsync(2, 3, 4);

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.WorkoutStreak.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenMultipleWorkoutsOnSameDay_ShouldCountDayOnce()
    {
        await SeedDaysAgoAsync(0, 0, 0, 1);

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.WorkoutStreak.Should().Be(2);
        result.WorkoutCount.Should().Be(4);
    }

    [Fact]
    public async Task Handle_WhenStreakIsLongerThanTenDays_ShouldCapAtTen()
    {
        await SeedDaysAgoAsync(Enumerable.Range(0, 15).ToArray());

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.WorkoutStreak.Should().Be(10);
    }

    [Fact]
    public async Task Handle_WhenOnlyOtherUserHasRecentWorkouts_ShouldNotCountThemInStreak()
    {
        _context.Workouts.AddRange(
            Workout(DaysAgo(0), ownerId: OtherUserId),
            Workout(DaysAgo(10)));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.WorkoutStreak.Should().Be(0);
        result.WorkoutCount.Should().Be(1);
    }
}
