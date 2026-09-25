using AwesomeAssertions;
using MixxFit.API.Features.WeightEntries.GetWeightLogs;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WeightEntries.WeightEntryTestData;

namespace MixxFit.UnitTests.Features.WeightEntries;

public class GetWeightLogsTests : IDisposable
{
    private static readonly int CurrentYear = DateTime.UtcNow.Year;

    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetWeightLogsHandler _handler;

    public GetWeightLogsTests()
    {
        _handler = new GetWeightLogsHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private void Seed()
    {
        _context.WeightEntries.AddRange(
            Entry(90m, Utc(2024, 11, 3)),
            Entry(89m, Utc(2024, 11, 20)),
            Entry(88m, Utc(2024, 12, 1)),
            Entry(87m, Utc(2024, 12, 31, hour: 23)),
            Entry(86m, Utc(2025, 1, 1)),
            Entry(84m, Utc(CurrentYear, 1, 10)),
            Entry(83m, Utc(CurrentYear, 1, 20)),
            Entry(82m, Utc(CurrentYear, 2, 5)),
            Entry(60m, Utc(2024, 12, 15), OtherUserId),
            Entry(61m, Utc(CurrentYear, 3, 1), OtherUserId));
        _context.SaveChanges();
    }

    [Fact]
    public async Task Handle_WhenUserHasNoEntries_ShouldReturnEmptyList()
    {
        var result = await _handler.Handle(UserId, new GetWeightLogsRequest());

        result.WeightLogs.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithYearAndMonth_ShouldReturnOnlyUsersEntriesForThatMonthNewestFirst()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightLogsRequest { Year = 2024, Month = 12 });

        result.WeightLogs.Select(w => w.Weight).Should().Equal(87m, 88m);
    }

    [Fact]
    public async Task Handle_WithYearOnly_ShouldReturnLatestMonthWithEntriesInThatYear()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightLogsRequest { Year = 2024 });

        result.WeightLogs.Select(w => w.CreatedAt.Month).Should().OnlyContain(m => m == 12);
        result.WeightLogs.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WithoutYearAndMonth_ShouldReturnLatestMonthOfCurrentYear()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightLogsRequest());

        result.WeightLogs.Select(w => w.Weight).Should().Equal(82m);
    }

    [Fact]
    public async Task Handle_WithMonthOnly_ShouldUseCurrentYear()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightLogsRequest { Month = 1 });

        result.WeightLogs.Select(w => w.Weight).Should().Equal(83m, 84m);
    }

    [Fact]
    public async Task Handle_WhenRequestedYearHasNoEntries_ShouldReturnEmptyList()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightLogsRequest { Year = 2020 });

        result.WeightLogs.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenRequestedMonthHasNoEntries_ShouldReturnEmptyList()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightLogsRequest { Year = 2024, Month = 6 });

        result.WeightLogs.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldExcludeEntriesOnFirstInstantOfNextMonth()
    {
        Seed();

        var result = await _handler.Handle(UserId, new GetWeightLogsRequest { Year = 2024, Month = 12 });

        result.WeightLogs.Should().NotContain(w => w.CreatedAt == Utc(2025, 1, 1));
    }

    [Fact]
    public async Task Handle_ShouldMapEntryFields()
    {
        var entry = Entry(81.3m, Utc(2024, 5, 5, hour: 6), notes: "Fasted");
        _context.WeightEntries.Add(entry);
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, new GetWeightLogsRequest { Year = 2024, Month = 5 });

        var record = result.WeightLogs.Should().ContainSingle().Subject;
        record.Id.Should().Be(entry.Id);
        record.Weight.Should().Be(81.3m);
        record.TimeLogged.Should().Be(TimeSpan.FromHours(6));
        record.CreatedAt.Should().Be(Utc(2024, 5, 5, hour: 6));
        record.Notes.Should().Be("Fasted");
    }
}
