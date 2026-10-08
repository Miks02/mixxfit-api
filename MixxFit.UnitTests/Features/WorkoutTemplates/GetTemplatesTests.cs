using AwesomeAssertions;
using MixxFit.API.Features.WorkoutTemplates.Common;
using MixxFit.API.Features.WorkoutTemplates.GetTemplates;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.WorkoutTemplates.WorkoutTemplateTestData;

namespace MixxFit.UnitTests.Features.WorkoutTemplates;

public class GetTemplatesTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetTemplatesHandler _handler;

    public GetTemplatesTests()
    {
        _handler = new GetTemplatesHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenNoTemplatesExist_ShouldReturnEmptyList()
    {
        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSystemAndOwnTemplatesButNotOtherUsers()
    {
        _context.WorkoutTemplates.AddRange(
            Template("System", ownerId: null, null, (1, 3, 1)),
            Template("Mine", UserId, null, (2, 3, 1)),
            Template("Theirs", OtherUserId, null, (3, 3, 1)));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.Select(t => (t.Name, t.IsSystem)).Should().BeEquivalentTo([("System", true), ("Mine", false)]);
    }

    [Fact]
    public async Task Handle_ShouldMapTemplateWithExercisesInOrder()
    {
        var template = Template("Mine", UserId, "Notes", (9, 2, 2), (8, 4, 1));
        _context.WorkoutTemplates.Add(template);
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, CancellationToken.None);

        var mapped = result.Should().ContainSingle().Subject;
        mapped.Id.Should().Be(template.Id);
        mapped.Name.Should().Be("Mine");
        mapped.Notes.Should().Be("Notes");
        mapped.IsSystem.Should().BeFalse();
        mapped.Exercises.Should().Equal(
            new TemplateExerciseDto { ExerciseId = 8, SetCount = 4, Order = 1 },
            new TemplateExerciseDto { ExerciseId = 9, SetCount = 2, Order = 2 });
    }
}
