using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Features.WorkoutTemplates.DeleteTemplate;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WorkoutTemplates.WorkoutTemplateTestData;

namespace MixxFit.UnitTests.Features.WorkoutTemplates;

// The handler relies on the database cascade to remove template exercises, which the EF InMemory provider does not
// perform, so these tests run against in-memory SQLite. SQLite also seeds the system exercises and templates.
public class DeleteTemplateTests : IDisposable
{
    private const int SystemTemplateId = 1;

    private readonly SqliteTestDatabase _database = new();
    private readonly int _templateId;
    private readonly int _otherUserTemplateId;

    public DeleteTemplateTests()
    {
        using var context = _database.CreateContext();

        context.Users.AddRange(UserWithProfile(UserId), UserWithProfile(OtherUserId));

        var template = Template("Push Day", UserId, null, (1, 3, 1), (2, 4, 2));
        var otherTemplate = Template("Their Day", OtherUserId, null, (1, 3, 1));

        context.WorkoutTemplates.AddRange(template, otherTemplate);
        context.SaveChanges();

        _templateId = template.Id;
        _otherUserTemplateId = otherTemplate.Id;
    }

    public void Dispose() => _database.Dispose();

    [Fact]
    public async Task Handle_WhenTemplateBelongsToUser_ShouldDeleteTemplateWithExercises()
    {
        await using var context = _database.CreateContext();

        var result = await new DeleteTemplateHandler(context).Handle(UserId, _templateId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var assertContext = _database.CreateContext();
        (await assertContext.WorkoutTemplates.AnyAsync(wt => wt.Id == _templateId)).Should().BeFalse();
        (await assertContext.WorkoutTemplateExercises.AnyAsync(wte => wte.WorkoutTemplateId == _templateId)).Should().BeFalse();
        (await assertContext.WorkoutTemplateExercises.AnyAsync(wte => wte.WorkoutTemplateId == _otherUserTemplateId)).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenTemplateBelongsToAnotherUser_ShouldReturnNotFoundAndKeepTemplate()
    {
        await using var context = _database.CreateContext();

        var result = await new DeleteTemplateHandler(context).Handle(UserId, _otherUserTemplateId, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.WorkoutTemplates.AnyAsync(wt => wt.Id == _otherUserTemplateId)).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenTemplateIsSystemTemplate_ShouldReturnNotFoundAndKeepTemplate()
    {
        await using var context = _database.CreateContext();

        var result = await new DeleteTemplateHandler(context).Handle(UserId, SystemTemplateId, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.WorkoutTemplates.AnyAsync(wt => wt.Id == SystemTemplateId)).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenTemplateDoesNotExist_ShouldReturnNotFound()
    {
        await using var context = _database.CreateContext();

        var result = await new DeleteTemplateHandler(context).Handle(UserId, 9999, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);
    }

    [Fact]
    public async Task Handle_WhenCalledTwice_ShouldReturnNotFoundOnSecondCall()
    {
        await using var context = _database.CreateContext();
        var handler = new DeleteTemplateHandler(context);

        await handler.Handle(UserId, _templateId, CancellationToken.None);
        var second = await handler.Handle(UserId, _templateId, CancellationToken.None);

        second.IsSuccess.Should().BeFalse();
        second.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);
    }

    [Fact]
    public async Task Handle_WhenFitnessProfileDoesNotExist_ShouldReturnFitnessProfileNotFound()
    {
        await using var context = _database.CreateContext();

        var result = await new DeleteTemplateHandler(context).Handle("no-profile", _templateId, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(FitnessProfileError.NotFound().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.WorkoutTemplates.AnyAsync(wt => wt.Id == _templateId)).Should().BeTrue();
    }
}
