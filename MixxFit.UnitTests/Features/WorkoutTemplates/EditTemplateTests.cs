using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Features.WorkoutTemplates.Common;
using MixxFit.API.Features.WorkoutTemplates.EditTemplate;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WorkoutTemplates.WorkoutTemplateTestData;

namespace MixxFit.UnitTests.Features.WorkoutTemplates;

// Replacing template exercises deletes and re-inserts rows with the same composite key, so these tests run against
// in-memory SQLite to exercise real change tracking and FK behaviour. SQLite also seeds the system exercises and templates.
public class EditTemplateTests : IDisposable
{
    private const int SystemTemplateId = 1;
    private const int OwnExerciseId = 501;
    private const int OtherUserExerciseId = 502;
    private const int DeletedExerciseId = 503;

    private readonly SqliteTestDatabase _database = new();
    private readonly int _templateId;
    private readonly int _otherUserTemplateId;

    public EditTemplateTests()
    {
        using var context = _database.CreateContext();

        context.Users.AddRange(UserWithProfile(UserId), UserWithProfile(OtherUserId));
        context.Exercises.AddRange(
            Exercise(OwnExerciseId, UserId),
            Exercise(OtherUserExerciseId, OtherUserId),
            Exercise(DeletedExerciseId, isDeleted: true));

        var template = Template("Push Day", UserId, "Old notes", (1, 3, 1), (2, 4, 2));
        var otherTemplate = Template("Their Day", OtherUserId, null, (1, 3, 1));
        var secondTemplate = Template("Pull Day", UserId, null, (2, 3, 1));

        context.WorkoutTemplates.AddRange(template, otherTemplate, secondTemplate);
        context.SaveChanges();

        _templateId = template.Id;
        _otherUserTemplateId = otherTemplate.Id;
    }

    public void Dispose() => _database.Dispose();

    private EditTemplateRequest Request(string name = "Push Day v2", string? notes = "New notes", params (int ExerciseId, int SetCount)[] exercises) => new()
    {
        Id = _templateId,
        Name = name,
        Notes = notes,
        Exercises = (exercises.Length == 0 ? [(1, 3), (2, 4)] : exercises)
            .Select(e => new EditTemplateRequest.ExerciseItem { ExerciseId = e.ExerciseId, SetCount = e.SetCount })
            .ToList()
    };

    private async Task<List<TemplateExerciseDto>> SavedExercisesAsync(int templateId)
    {
        await using var context = _database.CreateContext();

        return await context.WorkoutTemplateExercises
            .Where(wte => wte.WorkoutTemplateId == templateId)
            .OrderBy(wte => wte.Order)
            .Select(wte => new TemplateExerciseDto { ExerciseId = wte.ExerciseId, SetCount = wte.SetCount, Order = wte.Order })
            .ToListAsync();
    }

    [Fact]
    public async Task Handle_WhenExercisesAreUnchanged_ShouldUpdateNameAndNotesAndKeepExercises()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Id.Should().Be(_templateId);
        result.Payload.Name.Should().Be("Push Day v2");
        result.Payload.Notes.Should().Be("New notes");
        result.Payload.Exercises.Should().Equal(
            new TemplateExerciseDto { ExerciseId = 1, SetCount = 3, Order = 1 },
            new TemplateExerciseDto { ExerciseId = 2, SetCount = 4, Order = 2 });

        await using var assertContext = _database.CreateContext();
        var saved = await assertContext.WorkoutTemplates.SingleAsync(wt => wt.Id == _templateId);
        saved.Name.Should().Be("Push Day v2");
        saved.Notes.Should().Be("New notes");
        (await SavedExercisesAsync(_templateId)).Should().Equal(result.Payload.Exercises);
    }

    [Fact]
    public async Task Handle_WhenSetCountChanges_ShouldReplaceExercises()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(exercises: [(1, 5), (2, 4)]), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await SavedExercisesAsync(_templateId)).Should().Equal(
            new TemplateExerciseDto { ExerciseId = 1, SetCount = 5, Order = 1 },
            new TemplateExerciseDto { ExerciseId = 2, SetCount = 4, Order = 2 });
    }

    [Fact]
    public async Task Handle_WhenExercisesAreReordered_ShouldUpdateOrder()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(exercises: [(2, 4), (1, 3)]), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Exercises.Select(e => (e.ExerciseId, e.Order)).Should().Equal((2, 1), (1, 2));
        (await SavedExercisesAsync(_templateId)).Should().Equal(result.Payload.Exercises);
    }

    [Fact]
    public async Task Handle_WhenExercisesAreAddedAndRemoved_ShouldReplaceExercises()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(exercises: [(3, 2), (1, 3), (4, 5)]), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await SavedExercisesAsync(_templateId)).Should().Equal(
            new TemplateExerciseDto { ExerciseId = 3, SetCount = 2, Order = 1 },
            new TemplateExerciseDto { ExerciseId = 1, SetCount = 3, Order = 2 },
            new TemplateExerciseDto { ExerciseId = 4, SetCount = 5, Order = 3 });
    }

    [Fact]
    public async Task Handle_WhenNotesAreCleared_ShouldSaveNullNotes()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(notes: null), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var assertContext = _database.CreateContext();
        (await assertContext.WorkoutTemplates.SingleAsync(wt => wt.Id == _templateId)).Notes.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenTemplateBelongsToAnotherUser_ShouldReturnNotFoundAndKeepTemplate()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request() with { Id = _otherUserTemplateId }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.WorkoutTemplates.SingleAsync(wt => wt.Id == _otherUserTemplateId)).Name.Should().Be("Their Day");
    }

    [Fact]
    public async Task Handle_WhenTemplateIsSystemTemplate_ShouldReturnNotFoundAndKeepTemplate()
    {
        await using var context = _database.CreateContext();
        var originalName = (await context.WorkoutTemplates.AsNoTracking().SingleAsync(wt => wt.Id == SystemTemplateId)).Name;

        var result = await new EditTemplateHandler(context).Handle(UserId, Request() with { Id = SystemTemplateId }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.WorkoutTemplates.SingleAsync(wt => wt.Id == SystemTemplateId)).Name.Should().Be(originalName);
    }

    [Fact]
    public async Task Handle_WhenTemplateDoesNotExist_ShouldReturnNotFound()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request() with { Id = 9999 }, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);
    }

    [Theory]
    [InlineData("Pull Day")]
    [InlineData("pull day")]
    [InlineData("  PULL DAY  ")]
    public async Task Handle_WhenAnotherOwnTemplateHasSameName_ShouldReturnAlreadyExistsAndKeepTemplate(string name)
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(name: name), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.AlreadyExists().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.WorkoutTemplates.SingleAsync(wt => wt.Id == _templateId)).Name.Should().Be("Push Day");
    }

    [Fact]
    public async Task Handle_WhenKeepingOwnNameWithDifferentCasing_ShouldUpdateTemplate()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(name: "PUSH DAY"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Name.Should().Be("PUSH DAY");
    }

    [Fact]
    public async Task Handle_WhenAnotherUserHasTemplateWithSameName_ShouldUpdateTemplate()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(name: "Their Day"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenExerciseDoesNotExist_ShouldReturnExerciseNotFoundAndKeepExercises()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(exercises: [(1, 3), (9999, 2)]), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ExerciseError.NotFound().Code);
        result.Errors[0].Description.Should().EndWith(": 9999");
        await AssertTemplateUnchangedAsync();
    }

    [Theory]
    [InlineData(OtherUserExerciseId)]
    [InlineData(DeletedExerciseId)]
    public async Task Handle_WhenExerciseIsNotAvailableToUser_ShouldReturnExerciseNotFoundAndKeepExercises(int exerciseId)
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(exercises: [(exerciseId, 3)]), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ExerciseError.NotFound().Code);
        await AssertTemplateUnchangedAsync();
    }

    [Fact]
    public async Task Handle_WhenUsingOwnExercise_ShouldReplaceExercises()
    {
        await using var context = _database.CreateContext();

        var result = await new EditTemplateHandler(context).Handle(UserId, Request(exercises: [(OwnExerciseId, 3)]), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await SavedExercisesAsync(_templateId)).Should().Equal(new TemplateExerciseDto { ExerciseId = OwnExerciseId, SetCount = 3, Order = 1 });
    }

    private async Task AssertTemplateUnchangedAsync()
    {
        await using var assertContext = _database.CreateContext();
        var saved = await assertContext.WorkoutTemplates.SingleAsync(wt => wt.Id == _templateId);
        saved.Name.Should().Be("Push Day");
        saved.Notes.Should().Be("Old notes");
        (await SavedExercisesAsync(_templateId)).Should().Equal(
            new TemplateExerciseDto { ExerciseId = 1, SetCount = 3, Order = 1 },
            new TemplateExerciseDto { ExerciseId = 2, SetCount = 4, Order = 2 });
    }
}
