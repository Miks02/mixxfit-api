using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Features.WeightEntries.LogWeight;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WeightEntries.WeightEntryTestData;

namespace MixxFit.UnitTests.Features.WeightEntries;

public class LogWeightTests : IDisposable
{
    private readonly SqliteTestDatabase _database = new();

    public LogWeightTests()
    {
        using var context = _database.CreateContext();

        context.Users.AddRange(UserWithProfile(UserId, weight: 90m), UserWithProfile(OtherUserId, weight: 70m));
        context.SaveChanges();
    }

    public void Dispose() => _database.Dispose();

    private static LogWeightRequest Request(decimal weight = 85.5m, string? notes = "Morning") => new()
    {
        Weight = weight,
        Time = TimeSpan.FromHours(8),
        Notes = notes
    };

    private static LogWeightHandler CreateHandler(AppDbContext context)
        => new(context, NullLogger<LogWeightHandler>.Instance);

    private async Task SeedEntryAsync(decimal weight, DateTime createdAt, string ownerId = UserId)
    {
        await using var context = _database.CreateContext();
        context.WeightEntries.Add(Entry(weight, createdAt, ownerId));
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WhenNoWeightLoggedToday_ShouldCreateEntryAndReturnIt()
    {
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Id.Should().BePositive();
        result.Payload.Weight.Should().Be(85.5m);
        result.Payload.Time.Should().Be(TimeSpan.FromHours(8));
        result.Payload.Notes.Should().Be("Morning");
        result.Payload.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

        await using var assertContext = _database.CreateContext();
        var entry = await assertContext.WeightEntries.SingleAsync();
        entry.Id.Should().Be(result.Payload.Id);
        entry.OwnerId.Should().Be(UserId);
        entry.Weight.Should().Be(85.5m);
    }

    [Fact]
    public async Task Handle_WhenWeightIsLogged_ShouldUpdateOnlyUsersFitnessProfileWeight()
    {
        await using var context = _database.CreateContext();

        await CreateHandler(context).Handle(UserId, Request(weight: 82m), CancellationToken.None);

        await using var assertContext = _database.CreateContext();
        (await assertContext.FitnessProfiles.SingleAsync(fp => fp.UserId == UserId)).Weight.Should().Be(82m);
        (await assertContext.FitnessProfiles.SingleAsync(fp => fp.UserId == OtherUserId)).Weight.Should().Be(70m);
    }

    [Fact]
    public async Task Handle_WhenWeightAlreadyLoggedToday_ShouldReturnLimitReachedAndNotChangeAnything()
    {
        await SeedEntryAsync(88m, DateTime.UtcNow);
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, Request(weight: 80m), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WeightEntryError.LimitReached().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.WeightEntries.CountAsync()).Should().Be(1);
        (await assertContext.FitnessProfiles.SingleAsync(fp => fp.UserId == UserId)).Weight.Should().Be(90m);
    }

    [Fact]
    public async Task Handle_WhenWeightWasLoggedYesterday_ShouldCreateEntry()
    {
        await SeedEntryAsync(88m, DateTime.UtcNow.Date.AddDays(-1));
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await context.WeightEntries.CountAsync(w => w.OwnerId == UserId)).Should().Be(2);
    }

    [Fact]
    public async Task Handle_WhenAnotherUserLoggedWeightToday_ShouldCreateEntry()
    {
        await SeedEntryAsync(70m, DateTime.UtcNow, OtherUserId);
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await context.WeightEntries.CountAsync(w => w.OwnerId == UserId)).Should().Be(1);
    }

    [Fact]
    public async Task Handle_WhenFitnessProfileDoesNotExist_ShouldReturnFitnessProfileNotFoundAndNotCreateEntry()
    {
        // The handler returns before opening a transaction, so InMemory is enough and avoids seeding a user without a profile.
        await using var context = InMemoryTestDatabase.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(FitnessProfileError.NotFound().Code);
        (await context.WeightEntries.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenNotesAreNull_ShouldCreateEntryWithoutNotes()
    {
        await using var context = _database.CreateContext();

        var result = await CreateHandler(context).Handle(UserId, Request(notes: null), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Notes.Should().BeNull();
    }
}
