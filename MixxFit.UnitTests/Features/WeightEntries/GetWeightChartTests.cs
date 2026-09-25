using AwesomeAssertions;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.API.Features.WeightEntries.GetWeightChart.GetWeightChart;
using static MixxFit.UnitTests.Features.WeightEntries.WeightEntryTestData;

namespace MixxFit.UnitTests.Features.WeightEntries;

public class GetWeightChartTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly Handler _handler;

    public GetWeightChartTests()
    {
        _handler = new Handler(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenUserHasNoEntries_ShouldReturnEmptyEntriesWithTargetWeight()
    {
        var result = await _handler.Handle(UserId, 75, CancellationToken.None);

        result.Entries.Should().BeEmpty();
        result.TargetWeight.Should().Be(75);
    }

    [Fact]
    public async Task Handle_WhenUserHasEntries_ShouldReturnOnlyUsersEntriesNewestFirst()
    {
        _context.WeightEntries.AddRange(
            Entry(85m, Utc(2025, 12, 1)),
            Entry(80m, Utc(2026, 2, 1)),
            Entry(82m, Utc(2026, 1, 1)),
            Entry(60m, Utc(2026, 3, 1), OtherUserId));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, null, CancellationToken.None);

        result.Entries.Select(e => e.Weight).Should().Equal(80m, 82m, 85m);
        result.TargetWeight.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldMapEntryFields()
    {
        var entry = Entry(81.3m, Utc(2026, 1, 1, hour: 9), notes: "Fasted");
        _context.WeightEntries.Add(entry);
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, 70, CancellationToken.None);

        var record = result.Entries.Should().ContainSingle().Subject;
        record.Id.Should().Be(entry.Id);
        record.Weight.Should().Be(81.3m);
        record.TimeLogged.Should().Be(TimeSpan.FromHours(9));
        record.CreatedAt.Should().Be(Utc(2026, 1, 1, hour: 9));
        record.Notes.Should().Be("Fasted");
    }
}
