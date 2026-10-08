using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Features.WorkoutTemplates.Common;
using MixxFit.API.Features.WorkoutTemplates.CreateTemplate;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WorkoutTemplates.WorkoutTemplateTestData;

namespace MixxFit.UnitTests.Features.WorkoutTemplates;

public class CreateTemplateTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly CreateTemplateHandler _handler;

    public CreateTemplateTests()
    {
        _handler = new CreateTemplateHandler(_context);

        _context.FitnessProfiles.AddRange(new FitnessProfile { UserId = UserId }, new FitnessProfile { UserId = OtherUserId });
        _context.Exercises.AddRange(
            Exercise(1),
            Exercise(2),
            Exercise(3, ownerId: UserId),
            Exercise(4, isDeleted: true),
            Exercise(5, ownerId: OtherUserId));
        _context.SaveChanges();
    }

    public void Dispose() => _context.Dispose();

    private static CreateTemplateRequest Request(string name = "Push Day", string? notes = "Chest focus", params int[] exerciseIds) => new()
    {
        Name = name,
        Notes = notes,
        Exercises = (exerciseIds.Length == 0 ? [1, 2] : exerciseIds)
            .Select((id, index) => new CreateTemplateRequest.ExerciseItem { ExerciseId = id, SetCount = index + 3 })
            .ToList()
    };

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldCreateTemplateAndReturnIt()
    {
        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Id.Should().BePositive();
        result.Payload.Name.Should().Be("Push Day");
        result.Payload.Notes.Should().Be("Chest focus");
        result.Payload.Exercises.Should().Equal(
            new TemplateExerciseDto { ExerciseId = 1, SetCount = 3, Order = 1 },
            new TemplateExerciseDto { ExerciseId = 2, SetCount = 4, Order = 2 });

        var saved = await _context.WorkoutTemplates
            .Include(wt => wt.WorkoutTemplateExercises)
            .SingleAsync(wt => wt.Id == result.Payload.Id);
        saved.OwnerId.Should().Be(UserId);
        saved.WorkoutTemplateExercises.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ShouldAssignOrderFromRequestPosition()
    {
        var result = await _handler.Handle(UserId, Request(exerciseIds: [3, 1, 2]), CancellationToken.None);

        result.Payload!.Exercises.Select(e => (e.ExerciseId, e.Order)).Should().Equal((3, 1), (1, 2), (2, 3));
    }

    [Fact]
    public async Task Handle_WhenFitnessProfileDoesNotExist_ShouldReturnFitnessProfileNotFound()
    {
        var result = await _handler.Handle("no-profile", Request(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(FitnessProfileError.NotFound().Code);
        (await _context.WorkoutTemplates.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenUserHas20Templates_ShouldReturnLimitReached()
    {
        _context.WorkoutTemplates.AddRange(Enumerable.Range(1, 20).Select(i => Template($"Template {i}")));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.LimitReached().Code);
        (await _context.WorkoutTemplates.CountAsync()).Should().Be(20);
    }

    [Fact]
    public async Task Handle_WhenUserHas19Templates_ShouldCreateTemplate()
    {
        _context.WorkoutTemplates.AddRange(Enumerable.Range(1, 19).Select(i => Template($"Template {i}")));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await _context.WorkoutTemplates.CountAsync()).Should().Be(20);
    }

    [Fact]
    public async Task Handle_WhenOtherUsersAndSystemTemplatesExist_ShouldNotCountThemTowardsLimit()
    {
        _context.WorkoutTemplates.AddRange(Enumerable.Range(1, 20).Select(i => Template($"Other {i}", OtherUserId)));
        _context.WorkoutTemplates.AddRange(Enumerable.Range(1, 20).Select(i => Template($"System {i}", ownerId: null)));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("Push Day")]
    [InlineData("push day")]
    [InlineData("  PUSH DAY  ")]
    public async Task Handle_WhenUserHasTemplateWithSameName_ShouldReturnAlreadyExists(string name)
    {
        _context.WorkoutTemplates.Add(Template("Push Day"));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(name: name), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.AlreadyExists().Code);
        (await _context.WorkoutTemplates.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task Handle_WhenAnotherUserHasTemplateWithSameName_ShouldCreateTemplate()
    {
        _context.WorkoutTemplates.Add(Template("Push Day", OtherUserId));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(name: "Push Day"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenSystemTemplateHasSameName_ShouldCreateTemplate()
    {
        _context.WorkoutTemplates.Add(Template("Push Day", ownerId: null));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(name: "Push Day"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenExercisesDoNotExist_ShouldReturnExerciseNotFoundWithInvalidIds()
    {
        var result = await _handler.Handle(UserId, Request(exerciseIds: [1, 998, 999]), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ExerciseError.NotFound().Code);
        result.Errors[0].Description.Should().EndWith("998, 999");
        (await _context.WorkoutTemplates.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenExerciseIsDeleted_ShouldReturnExerciseNotFound()
    {
        var result = await _handler.Handle(UserId, Request(exerciseIds: [1, 4]), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ExerciseError.NotFound().Code);
    }

    [Fact]
    public async Task Handle_WhenExerciseBelongsToAnotherUser_ShouldReturnExerciseNotFoundAndNotCreateTemplate()
    {
        var result = await _handler.Handle(UserId, Request(exerciseIds: [1, 5]), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ExerciseError.NotFound().Code);
        result.Errors[0].Description.Should().EndWith(": 5");
        (await _context.WorkoutTemplates.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenUsingOwnAndSystemExercises_ShouldCreateTemplate()
    {
        var result = await _handler.Handle(UserId, Request(exerciseIds: [1, 3]), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenNotesAreNull_ShouldCreateTemplateWithoutNotes()
    {
        var result = await _handler.Handle(UserId, Request(notes: null), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Notes.Should().BeNull();
    }
}
