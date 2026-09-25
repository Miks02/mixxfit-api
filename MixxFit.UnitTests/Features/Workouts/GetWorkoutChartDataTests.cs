using AwesomeAssertions;
using MixxFit.API.Features.Workouts.GetWorkoutChartData;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Workouts.WorkoutTestData;

namespace MixxFit.UnitTests.Features.Workouts;

public class GetWorkoutChartDataTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetWorkoutChartDataHandler _handler;

    public GetWorkoutChartDataTests()
    {
        _handler = new GetWorkoutChartDataHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private void Seed()
    {
        _context.Workouts.AddRange(
            Workout(Utc(2024, 6, 1)),
            Workout(Utc(2025, 1, 5)),
            Workout(Utc(2025, 1, 20)),
            Workout(Utc(2025, 3, 3)),
            Workout(Utc(2025, 12, 31)),
            Workout(Utc(2025, 12, 1)),
            Workout(Utc(2025, 12, 15)),
            Workout(Utc(2026, 7, 7), ownerId: OtherUserId));
        _context.SaveChanges();
    }

    private static int[] Months(GetWorkoutChartDataResponse r) =>
    [
        r.JanuaryWorkouts, r.FebruaryWorkouts, r.MarchWorkouts, r.AprilWorkouts, r.MayWorkouts, r.JuneWorkouts,
        r.JulyWorkouts, r.AugustWorkouts, r.SeptemberWorkouts, r.OctoberWorkouts, r.NovemberWorkouts, r.DecemberWorkouts
    ];

    [Fact]
    public async Task Handle_WhenUserHasNoWorkouts_ShouldReturnZeroes()
    {
        var result = await _handler.Handle(UserId, new GetWorkoutChartDataRequest(), CancellationToken.None);

        result.Years.Should().BeEmpty();
        Months(result).Should().AllBeEquivalentTo(0);
    }

    [Fact]
    public async Task Handle_WithoutYear_ShouldUseLatestYearAndReturnYearsDescending()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutChartDataRequest(), CancellationToken.None);

        result.Years.Should().Equal(2025, 2024);
        Months(result).Should().Equal(2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 3);
    }

    [Fact]
    public async Task Handle_WithYear_ShouldReturnMonthlyCountsForThatYear()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutChartDataRequest { Year = 2024 }, CancellationToken.None);

        result.Years.Should().Equal(2025, 2024);
        Months(result).Should().Equal(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0);
    }

    [Fact]
    public async Task Handle_WhenRequestedYearHasNoWorkouts_ShouldReturnZeroes()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWorkoutChartDataRequest { Year = 2026 }, CancellationToken.None);

        result.Years.Should().Equal(2025, 2024);
        Months(result).Should().AllBeEquivalentTo(0);
    }
}
