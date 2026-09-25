using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.ExerciseEntries;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Exercises.DeleteExercise;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Exercises.ExerciseTestData;

namespace MixxFit.UnitTests.Features.Exercises;

public class DeleteExerciseTests : IDisposable
{
    private const int UnusedExerciseId = 100;
    private const int UsedExerciseId = 101;
    private const int OtherUserExerciseId = 200;
    private const int SystemExerciseId = 300;
    private const int DeletedExerciseId = 400;

    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly DeleteExerciseHandler _handler;

    public DeleteExerciseTests()
    {
        SeedLookups(_context, UserId, OtherUserId);
        _context.Exercises.AddRange(
            Exercise(UnusedExerciseId, "Unused"),
            Exercise(UsedExerciseId, "Used"),
            Exercise(OtherUserExerciseId, "Theirs", OtherUserId),
            Exercise(SystemExerciseId, "System", ownerId: null),
            Exercise(DeletedExerciseId, "Deleted", isDeleted: true));
        _context.Workouts.Add(new Workout
        {
            Name = "Workout",
            OwnerId = UserId,
            ExerciseEntries =
            [
                new ExerciseEntry { ExerciseId = UsedExerciseId, Name = "Used", ExerciseType = ExerciseType.WeightLifting }
            ]
        });
        _context.SaveChanges();
        _context.ChangeTracker.Clear();

        _handler = new DeleteExerciseHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenExerciseIsNotUsedInAnyWorkout_ShouldRemoveIt()
    {
        var result = await _handler.Handle(UserId, UnusedExerciseId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await _context.Exercises.IgnoreQueryFilters().AnyAsync(e => e.Id == UnusedExerciseId)).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenExerciseIsUsedInWorkout_ShouldSoftDeleteIt()
    {
        var result = await _handler.Handle(UserId, UsedExerciseId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var exercise = await _context.Exercises.IgnoreQueryFilters().AsNoTracking().SingleAsync(e => e.Id == UsedExerciseId);
        exercise.IsDeleted.Should().BeTrue();
        (await _context.Exercises.AnyAsync(e => e.Id == UsedExerciseId)).Should().BeFalse("deleted exercises are hidden by the query filter");
        (await _context.ExerciseEntries.AnyAsync(e => e.ExerciseId == UsedExerciseId)).Should().BeTrue("history must be preserved");
    }

    [Theory]
    [InlineData(999)]
    [InlineData(DeletedExerciseId)]
    public async Task Handle_WhenExerciseDoesNotExistOrIsAlreadyDeleted_ShouldReturnNotFound(int id)
    {
        var result = await _handler.Handle(UserId, id, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ExerciseError.NotFound().Code);
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be($"Exercise with id '{id}' was not found");
    }

    [Theory]
    [InlineData(OtherUserExerciseId)]
    [InlineData(SystemExerciseId)]
    public async Task Handle_WhenExerciseIsNotOwnedByUser_ShouldReturnForbiddenAndKeepIt(int id)
    {
        var result = await _handler.Handle(UserId, id, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(GeneralError.Forbidden().Code);
        result.Errors[0].Type.Should().Be(ErrorType.Forbidden);

        var exercise = await _context.Exercises.AsNoTracking().SingleAsync(e => e.Id == id);
        exercise.IsDeleted.Should().BeFalse();
    }
}
