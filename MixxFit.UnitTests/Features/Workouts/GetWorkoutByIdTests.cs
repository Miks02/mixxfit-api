using AwesomeAssertions;
using MixxFit.API.Domain.Entities.SetEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Workouts.GetWorkoutById;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Workouts.WorkoutTestData;

namespace MixxFit.UnitTests.Features.Workouts;

public class GetWorkoutByIdTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetWorkoutByIdHandler _handler;

    public GetWorkoutByIdTests()
    {
        _handler = new GetWorkoutByIdHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private async Task<Workout> SeedAsync(Workout workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        return workout;
    }

    [Fact]
    public async Task Handle_WhenWorkoutDoesNotExist_ShouldReturnNotFound()
    {
        var result = await _handler.Handle(UserId, 42, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutError.NotFound().Code);
    }

    [Fact]
    public async Task Handle_WhenWorkoutBelongsToAnotherUser_ShouldReturnNotFound()
    {
        var workout = await SeedAsync(Workout(Utc(2026, 1, 1), ownerId: OtherUserId));

        var result = await _handler.Handle(UserId, workout.Id, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutError.NotFound().Code);
    }

    [Fact]
    public async Task Handle_WhenWorkoutBelongsToUser_ShouldReturnMappedWorkout()
    {
        var workout = Workout(Utc(2026, 3, 15), "Leg Day", UserId,
            Entry(ExerciseType.WeightLifting, sets: 2, name: "Squat"));
        workout.Notes = "Heavy";
        workout.CreatedAt = Utc(2026, 3, 16);
        await SeedAsync(workout);

        var result = await _handler.Handle(UserId, workout.Id, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var response = result.Payload!;
        response.Id.Should().Be(workout.Id);
        response.Name.Should().Be("Leg Day");
        response.Notes.Should().Be("Heavy");
        response.WorkoutDate.Should().Be(Utc(2026, 3, 15));
        response.CreatedAt.Should().Be(Utc(2026, 3, 16));

        var exercise = response.Exercises.Should().ContainSingle().Subject;
        exercise.Id.Should().Be(workout.ExerciseEntries.Single().Id);
        exercise.Name.Should().Be("Squat");
        exercise.ExerciseType.Should().Be(ExerciseType.WeightLifting);
        exercise.Sets.Should().HaveCount(2).And.AllSatisfy(s =>
        {
            s.Reps.Should().Be(10);
            s.Weight.Should().Be(50m);
        });
    }

    [Theory]
    [InlineData(150, 2, 30)]
    [InlineData(59, 0, 59)]
    [InlineData(3600, 60, 0)]
    public async Task Handle_WhenSetHasDuration_ShouldSplitIntoMinutesAndSeconds(int totalSeconds, int expectedMinutes, int expectedSeconds)
    {
        var entry = Entry(ExerciseType.Cardio, sets: 0);
        entry.Sets.Add(new SetEntry { DurationSeconds = totalSeconds, Distance = 2.5m });
        var workout = await SeedAsync(Workout(Utc(2026, 1, 1), entries: entry));

        var result = await _handler.Handle(UserId, workout.Id, CancellationToken.None);

        var set = result.Payload!.Exercises.Single().Sets.Single();
        set.DurationMinutes.Should().Be(expectedMinutes);
        set.DurationSeconds.Should().Be(expectedSeconds);
        set.Distance.Should().Be(2.5m);
    }

    [Fact]
    public async Task Handle_WhenSetHasNoDuration_ShouldReturnNullDuration()
    {
        var workout = await SeedAsync(Workout(Utc(2026, 1, 1), entries: Entry(ExerciseType.WeightLifting)));

        var result = await _handler.Handle(UserId, workout.Id, CancellationToken.None);

        var set = result.Payload!.Exercises.Single().Sets.Single();
        set.DurationMinutes.Should().BeNull();
        set.DurationSeconds.Should().BeNull();
    }
}
