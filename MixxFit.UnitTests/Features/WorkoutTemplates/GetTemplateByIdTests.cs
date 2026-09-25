using AwesomeAssertions;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Features.WorkoutTemplates.Common;
using MixxFit.API.Features.WorkoutTemplates.GetTemplateById;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WorkoutTemplates.WorkoutTemplateTestData;

namespace MixxFit.UnitTests.Features.WorkoutTemplates;

public class GetTemplateByIdTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetTemplateByIdHandler _handler;

    public GetTemplateByIdTests()
    {
        _handler = new GetTemplateByIdHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private async Task<int> SeedAsync(WorkoutTemplate template)
    {
        _context.WorkoutTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template.Id;
    }

    [Fact]
    public async Task Handle_WhenTemplateBelongsToUser_ShouldReturnMappedTemplateWithExercisesInOrder()
    {
        var id = await SeedAsync(Template("Push Day", UserId, "Chest focus", (7, 2, 3), (5, 4, 1), (6, 3, 2)));

        var result = await _handler.Handle(UserId, id, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Id.Should().Be(id);
        result.Payload.Name.Should().Be("Push Day");
        result.Payload.Notes.Should().Be("Chest focus");
        result.Payload.IsSystem.Should().BeFalse();
        result.Payload.Exercises.Should().Equal(
            new TemplateExerciseDto { ExerciseId = 5, SetCount = 4, Order = 1 },
            new TemplateExerciseDto { ExerciseId = 6, SetCount = 3, Order = 2 },
            new TemplateExerciseDto { ExerciseId = 7, SetCount = 2, Order = 3 });
    }

    [Fact]
    public async Task Handle_WhenTemplateIsSystemTemplate_ShouldReturnItMarkedAsSystem()
    {
        var id = await SeedAsync(Template("Full Body", ownerId: null, null, (1, 3, 1)));

        var result = await _handler.Handle(UserId, id, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.IsSystem.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenTemplateBelongsToAnotherUser_ShouldReturnNotFound()
    {
        var id = await SeedAsync(Template("Their Day", OtherUserId, null, (1, 3, 1)));

        var result = await _handler.Handle(UserId, id, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);
    }

    [Fact]
    public async Task Handle_WhenTemplateDoesNotExist_ShouldReturnNotFound()
    {
        var result = await _handler.Handle(UserId, 9999, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutTemplateError.NotFound().Code);
    }
}
