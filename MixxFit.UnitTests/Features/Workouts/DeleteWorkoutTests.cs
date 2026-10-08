using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Enums;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.API.Features.Workouts.DeleteWorkout.DeleteWorkout;
using static MixxFit.UnitTests.Features.Workouts.WorkoutTestData;

namespace MixxFit.UnitTests.Features.Workouts;

// ExecuteDeleteAsync is not supported by the EF InMemory provider, so these tests run against in-memory SQLite.
public class DeleteWorkoutTests : IDisposable
{
    private readonly SqliteTestDatabase _database = new();
    private readonly int _workoutId;
    private readonly int _otherUserWorkoutId;

    public DeleteWorkoutTests()
    {
        using var context = _database.CreateContext();

        context.Users.AddRange(
            new User { Id = UserId, UserName = "user1", Email = "user1@mail.com", FitnessProfile = new FitnessProfile { UserId = UserId } },
            new User { Id = OtherUserId, UserName = "user2", Email = "user2@mail.com", FitnessProfile = new FitnessProfile { UserId = OtherUserId } });

        var workout = Workout(Utc(2026, 1, 1), "Mine", UserId,
            Entry(ExerciseType.WeightLifting, sets: 3, exerciseId: 1),
            Entry(ExerciseType.WeightLifting, sets: 2, exerciseId: 2));
        var otherWorkout = Workout(Utc(2026, 1, 1), "Theirs", OtherUserId,
            Entry(ExerciseType.WeightLifting, sets: 1, exerciseId: 1));

        context.Workouts.AddRange(workout, otherWorkout);
        context.SaveChanges();

        _workoutId = workout.Id;
        _otherUserWorkoutId = otherWorkout.Id;
    }

    public void Dispose() => _database.Dispose();

    [Fact]
    public async Task Handle_WhenWorkoutBelongsToUser_ShouldDeleteWorkoutWithEntriesAndSets()
    {
        await using var context = _database.CreateContext();

        var result = await new DeleteWorkoutHandler(context).Handle(UserId, _workoutId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await using var assertContext = _database.CreateContext();
        (await assertContext.Workouts.AnyAsync(w => w.Id == _workoutId)).Should().BeFalse();
        (await assertContext.ExerciseEntries.AnyAsync(e => e.WorkoutId == _workoutId)).Should().BeFalse();
        (await assertContext.SetEntries.CountAsync()).Should().Be(1, "only the other user's set should remain");
    }

    [Fact]
    public async Task Handle_WhenWorkoutBelongsToAnotherUser_ShouldReturnNotFoundAndKeepWorkout()
    {
        await using var context = _database.CreateContext();

        var result = await new DeleteWorkoutHandler(context).Handle(UserId, _otherUserWorkoutId, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutError.NotFound().Code);

        await using var assertContext = _database.CreateContext();
        (await assertContext.Workouts.AnyAsync(w => w.Id == _otherUserWorkoutId)).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenWorkoutDoesNotExist_ShouldReturnNotFound()
    {
        await using var context = _database.CreateContext();

        var result = await new DeleteWorkoutHandler(context).Handle(UserId, 9999, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutError.NotFound().Code);
        (await context.Workouts.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task Handle_WhenCalledTwice_ShouldReturnNotFoundOnSecondCall()
    {
        await using var context = _database.CreateContext();
        var handler = new DeleteWorkoutHandler(context);

        await handler.Handle(UserId, _workoutId, CancellationToken.None);
        var second = await handler.Handle(UserId, _workoutId, CancellationToken.None);

        second.IsSuccess.Should().BeFalse();
        second.Errors[0].Code.Should().Be(WorkoutError.NotFound().Code);
    }
}
