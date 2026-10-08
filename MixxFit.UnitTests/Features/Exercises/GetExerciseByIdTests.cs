using AwesomeAssertions;
using MixxFit.API.Features.Exercises.GetExerciseById;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Exercises.ExerciseTestData;

namespace MixxFit.UnitTests.Features.Exercises;

public class GetExerciseByIdTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetExerciseByIdHandler _handler;

    public GetExerciseByIdTests()
    {
        SeedLookups(_context, UserId, OtherUserId);
        _context.Exercises.AddRange(
            Exercise(100, "Paused Bench", categoryId: DumbbellCategoryId, muscleGroupId: BackId),
            Exercise(200, "Theirs", OtherUserId),
            Exercise(300, "System", ownerId: null),
            Exercise(400, "Deleted", isDeleted: true));
        _context.SaveChanges();

        _handler = new GetExerciseByIdHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenExerciseBelongsToUser_ShouldReturnIt()
    {
        var result = await _handler.Handle(UserId, 100, CancellationToken.None);

        result.Should().Be(new GetExerciseByIdResponse
        {
            Name = "Paused Bench",
            CategoryId = DumbbellCategoryId,
            MuscleGroupId = BackId
        });
    }

    [Theory]
    [InlineData(999)]
    [InlineData(200)]
    [InlineData(300)]
    [InlineData(400)]
    public async Task Handle_WhenExerciseIsMissingNotOwnedOrDeleted_ShouldReturnNull(int id)
    {
        var result = await _handler.Handle(UserId, id, CancellationToken.None);

        result.Should().BeNull();
    }
}
