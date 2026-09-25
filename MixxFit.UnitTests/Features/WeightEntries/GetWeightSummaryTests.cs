using AwesomeAssertions;
using MixxFit.API.Features.WeightEntries.GetWeightSummary;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WeightEntries.WeightEntryTestData;

namespace MixxFit.UnitTests.Features.WeightEntries;

public class GetWeightSummaryTests : IDisposable
{
    private static readonly int CurrentYear = DateTime.UtcNow.Year;

    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetWeightSummaryHandler _handler;

    public GetWeightSummaryTests()
    {
        _handler = new GetWeightSummaryHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private void Seed()
    {
        _context.WeightEntries.AddRange(
            Entry(90m, Utc(2024, 11, 3), notes: "Start"),
            Entry(88m, Utc(2024, 12, 1)),
            Entry(87m, Utc(2024, 12, 20), notes: "Holidays"),
            Entry(84m, Utc(CurrentYear, 1, 10)),
            Entry(82.5m, Utc(CurrentYear, 2, 5)),
            Entry(60m, Utc(CurrentYear, 3, 1), OtherUserId));
        _context.SaveChanges();
    }

    [Fact]
    public async Task Handle_WhenUserHasNoEntries_ShouldReturnEmptySummary()
    {
        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest { TargetWeight = 75 }, CancellationToken.None);

        result.CurrentWeight.Should().BeNull();
        result.WeightDelta.Should().BeNull();
        result.WeightListDetails.WeightLogs.Should().BeEmpty();
        result.WeightChart.Entries.Should().BeEmpty();
        result.YearsAndMonthsGroup.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenUserHasEntries_ShouldReturnLatestEntryAsCurrentWeight()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest(), CancellationToken.None);

        result.CurrentWeight.Should().BeEquivalentTo(new CurrentWeightDto { Weight = 82.5m, CreatedAt = Utc(CurrentYear, 2, 5) });
    }

    [Fact]
    public async Task Handle_WhenUserHasMultipleEntries_ShouldReturnDeltaAgainstPreviousEntry()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest(), CancellationToken.None);

        result.WeightDelta.Should().BeEquivalentTo(new WeightDeltaDto { Delta = -1.5m, CreatedAt = Utc(CurrentYear, 1, 10) });
    }

    [Fact]
    public async Task Handle_WhenUserHasSingleEntry_ShouldReturnNullDelta()
    {
        _context.WeightEntries.AddRange(
            Entry(80m, Utc(2025, 5, 5)),
            Entry(60m, Utc(2025, 4, 4), OtherUserId));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest(), CancellationToken.None);

        result.CurrentWeight!.Weight.Should().Be(80m);
        result.WeightDelta.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnChartWithAllUsersEntriesNewestFirstAndTargetWeight()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest { TargetWeight = 78.5 }, CancellationToken.None);

        result.WeightChart.Entries.Select(e => e.Weight).Should().Equal(82.5m, 84m, 87m, 88m, 90m);
        result.WeightChart.TargetWeight.Should().Be(78.5);
    }

    [Fact]
    public async Task Handle_WithYearAndMonth_ShouldReturnLogsForThatMonthIncludingNotes()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest { Year = 2024, Month = 12 }, CancellationToken.None);

        result.WeightListDetails.WeightLogs.Select(w => w.Weight).Should().Equal(87m, 88m);
        result.WeightListDetails.WeightLogs[0].Notes.Should().Be("Holidays");
    }

    [Fact]
    public async Task Handle_WithYearOnly_ShouldReturnLogsForLatestMonthWithEntriesInThatYear()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest { Year = 2024 }, CancellationToken.None);

        result.WeightListDetails.WeightLogs.Select(w => w.Weight).Should().Equal(87m, 88m);
    }

    [Fact]
    public async Task Handle_WithoutYearAndMonth_ShouldReturnLogsForLatestMonthOfCurrentYear()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest(), CancellationToken.None);

        result.WeightListDetails.WeightLogs.Select(w => w.Weight).Should().Equal(82.5m);
    }

    [Fact]
    public async Task Handle_WhenRequestedYearHasNoEntries_ShouldReturnEmptyLogsButKeepOtherSections()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest { Year = 2020 }, CancellationToken.None);

        result.WeightListDetails.WeightLogs.Should().BeEmpty();
        result.CurrentWeight.Should().NotBeNull();
        result.WeightChart.Entries.Should().HaveCount(5);
    }

    [Fact]
    public async Task Handle_ShouldGroupAvailableYearsAndMonthsDescending()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest(), CancellationToken.None);

        result.YearsAndMonthsGroup.Keys.Should().Equal(CurrentYear, 2024);
        result.YearsAndMonthsGroup[CurrentYear].Should().Equal(2, 1);
        result.YearsAndMonthsGroup[2024].Should().Equal(12, 11);
    }

    [Fact]
    public async Task Handle_ShouldIgnoreOtherUsersEntries()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightSummaryRequest { Year = CurrentYear, Month = 3 }, CancellationToken.None);

        result.WeightListDetails.WeightLogs.Should().BeEmpty();
        result.WeightChart.Entries.Should().NotContain(e => e.Weight == 60m);
        result.YearsAndMonthsGroup[CurrentYear].Should().NotContain(3);
    }
}
