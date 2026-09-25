using AwesomeAssertions;
using MixxFit.API.Features.WeightEntries.GetWeightById;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WeightEntries.WeightEntryTestData;

namespace MixxFit.UnitTests.Features.WeightEntries;

public class GetWeightByIdTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetWeightByIdHandler _handler;

    public GetWeightByIdTests()
    {
        _handler = new GetWeightByIdHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenEntryBelongsToUser_ShouldReturnMappedEntry()
    {
        var entry = Entry(82.4m, Utc(2026, 3, 4, hour: 7), notes: "After run");
        _context.WeightEntries.Add(entry);
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, entry.Id, CancellationToken.None);

        result.Should().BeEquivalentTo(new GetWeightByIdResponse
        {
            Id = entry.Id,
            Weight = 82.4m,
            Time = TimeSpan.FromHours(7),
            CreatedAt = Utc(2026, 3, 4, hour: 7),
            Notes = "After run"
        });
    }

    [Fact]
    public async Task Handle_WhenEntryBelongsToAnotherUser_ShouldReturnNull()
    {
        var entry = Entry(70m, Utc(2026, 3, 4), OtherUserId);
        _context.WeightEntries.Add(entry);
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, entry.Id, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenEntryDoesNotExist_ShouldReturnNull()
    {
        var result = await _handler.Handle(UserId, 9999, CancellationToken.None);

        result.Should().BeNull();
    }
}
