using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Features.WeightEntries.DeleteWeight;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WeightEntries.WeightEntryTestData;

namespace MixxFit.UnitTests.Features.WeightEntries;

// The handler opens a transaction, which the EF InMemory provider does not support, so these tests run against in-memory SQLite.
public class DeleteWeightTests : IDisposable
{
    private readonly SqliteTestDatabase _database = new();
    private readonly int _olderEntryId;
    private readonly int _latestEntryId;
    private readonly int _otherUserEntryId;

    public DeleteWeightTests()
    {
        using var context = _database.CreateContext();

        context.Users.AddRange(UserWithProfile(UserId, weight: 80m), UserWithProfile(OtherUserId, weight: 70m));

        var older = Entry(85m, Utc(2026, 1, 1));
        var latest = Entry(80m, Utc(2026, 1, 10));
        var otherUser = Entry(70m, Utc(2026, 1, 5), OtherUserId);

        context.WeightEntries.AddRange(older, latest, otherUser);
        context.SaveChanges();

        _olderEntryId = older.Id;
        _latestEntryId = latest.Id;
        _otherUserEntryId = otherUser.Id;
    }

    public void Dispose() => _database.Dispose();

    private static DeleteWeightHandler CreateHandler(AppDbContext context)
        => new(context, NullLogger<DeleteWeightHandler>.Instance);

    [Fact]
    public async Task Handle_WhenDeletingLatestEntry_ShouldDeleteItAndSetProfileWeightToPreviousEntry()
    {
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, _latestEntryId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var assertContext = _database.CreateContext();
        (await assertContext.WeightEntries.AnyAsync(w => w.Id == _latestEntryId)).Should().BeFalse();
        (await assertContext.FitnessProfiles.SingleAsync(fp => fp.UserId == UserId)).Weight.Should().Be(85m);
    }

    [Fact]
    public async Task Handle_WhenDeletingOlderEntry_ShouldKeepProfileWeightAtLatestEntry()
    {
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, _olderEntryId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var assertContext = _database.CreateContext();
        (await assertContext.WeightEntries.AnyAsync(w => w.Id == _olderEntryId)).Should().BeFalse();
        (await assertContext.FitnessProfiles.SingleAsync(fp => fp.UserId == UserId)).Weight.Should().Be(80m);
    }

    [Fact]
    public async Task Handle_WhenDeletingLastRemainingEntry_ShouldSetProfileWeightToNull()
    {
        await using var context = _database.CreateContext();
        var handler = CreateHandler(context);

        await handler.Handle(UserId, _olderEntryId, CancellationToken.None);
        var result = await handler.Handle(UserId, _latestEntryId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var assertContext = _database.CreateContext();
        (await assertContext.WeightEntries.AnyAsync(w => w.OwnerId == UserId)).Should().BeFalse();
        (await assertContext.FitnessProfiles.SingleAsync(fp => fp.UserId == UserId)).Weight.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenEntryBelongsToAnotherUser_ShouldReturnNotFoundAndKeepEntry()
    {
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, _otherUserEntryId, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WeightEntryError.NotFound().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.WeightEntries.AnyAsync(w => w.Id == _otherUserEntryId)).Should().BeTrue();
        (await assertContext.FitnessProfiles.SingleAsync(fp => fp.UserId == OtherUserId)).Weight.Should().Be(70m);
    }

    [Fact]
    public async Task Handle_WhenEntryDoesNotExist_ShouldReturnNotFound()
    {
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, 9999, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WeightEntryError.NotFound().Code);
        (await context.WeightEntries.CountAsync()).Should().Be(3);
    }

    [Fact]
    public async Task Handle_WhenCalledTwice_ShouldReturnNotFoundOnSecondCall()
    {
        await using var context = _database.CreateContext();
        var handler = CreateHandler(context);

        await handler.Handle(UserId, _latestEntryId, CancellationToken.None);
        var second = await handler.Handle(UserId, _latestEntryId, CancellationToken.None);

        second.IsSuccess.Should().BeFalse();
        second.Errors[0].Code.Should().Be(WeightEntryError.NotFound().Code);
    }

    [Fact]
    public async Task Handle_WhenFitnessProfileDoesNotExist_ShouldReturnFitnessProfileNotFoundAndKeepEntry()
    {
        // An entry without a profile violates the FK in SQLite; the handler returns before opening a transaction, so InMemory is enough.
        await using var context = InMemoryTestDatabase.CreateContext();
        var entry = Entry(80m, Utc(2026, 1, 1));
        context.WeightEntries.Add(entry);
        await context.SaveChangesAsync();

        var result = await CreateHandler(context).Handle(UserId, entry.Id, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(FitnessProfileError.NotFound().Code);
        (await context.WeightEntries.AnyAsync(w => w.Id == entry.Id)).Should().BeTrue();
    }
}
