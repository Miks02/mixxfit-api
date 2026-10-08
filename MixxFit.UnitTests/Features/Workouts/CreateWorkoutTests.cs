using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Workouts.CreateWorkout;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Workouts.WorkoutTestData;

namespace MixxFit.UnitTests.Features.Workouts;

public class CreateWorkoutTests : IDisposable
{
    private const int BenchPressId = 1;
    private const int RunningId = 2;
    private const int DeletedExerciseId = 3;

    private readonly AppDbContext _context;
    private readonly CreateWorkoutHandler _handler;

    public CreateWorkoutTests()
    {
        _context = InMemoryTestDatabase.CreateContext();
        _context.FitnessProfiles.Add(new FitnessProfile { UserId = UserId });
        _context.Exercises.AddRange(
            new Exercise { Id = BenchPressId, Name = "Bench Press", ExerciseType = ExerciseType.WeightLifting },
            new Exercise { Id = RunningId, Name = "Running", ExerciseType = ExerciseType.Cardio },
            new Exercise { Id = DeletedExerciseId, Name = "Old", ExerciseType = ExerciseType.Other, IsDeleted = true });
        _context.SaveChanges();
        _context.ChangeTracker.Clear();

        _handler = new CreateWorkoutHandler(_context, NullLogger<CreateWorkoutHandler>.Instance);
    }

    public void Dispose() => _context.Dispose();

    private static CreateWorkoutRequest ValidRequest(params int[] exerciseIds) => new()
    {
        Name = "Push Day",
        Notes = "Felt strong",
        WorkoutDate = new DateTime(2026, 5, 10, 18, 45, 0, DateTimeKind.Local),
        ExerciseEntries = (exerciseIds.Length == 0 ? [BenchPressId] : exerciseIds)
            .Select(id => new ExerciseEntryDto
            {
                ExerciseId = id,
                Name = $"Exercise {id}",
                ExerciseType = ExerciseType.WeightLifting,
                Sets = [new SetEntryDto { Reps = 8, Weight = 80m }]
            })
            .ToList()
    };

    [Fact]
    public async Task Handle_WhenFitnessProfileDoesNotExist_ShouldReturnFitnessProfileNotFound()
    {
        var result = await _handler.Handle("missing-user", ValidRequest(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(FitnessProfileError.NotFound().Code);
        (await _context.Workouts.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenFiveWorkoutsAlreadyLoggedToday_ShouldReturnLimitReached()
    {
        for (var i = 0; i < 5; i++)
            _context.Workouts.Add(Workout(DateTime.UtcNow, $"Workout {i}"));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, ValidRequest(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutError.LimitReached().Code);
        result.Errors[0].Type.Should().Be(ErrorType.TooManyRequests);
        (await _context.Workouts.CountAsync()).Should().Be(5);
    }

    [Fact]
    public async Task Handle_WhenMoreThanFiveWorkoutsAlreadyLoggedToday_ShouldReturnLimitReached()
    {
        for (var i = 0; i < 6; i++)
            _context.Workouts.Add(Workout(DateTime.UtcNow, $"Workout {i}"));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, ValidRequest(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(WorkoutError.LimitReached().Code);
        (await _context.Workouts.CountAsync()).Should().Be(6);
    }

    [Fact]
    public async Task Handle_WhenFourWorkoutsAlreadyLoggedToday_ShouldAllowFifth()
    {
        for (var i = 0; i < 4; i++)
            _context.Workouts.Add(Workout(DateTime.UtcNow, $"Workout {i}"));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, ValidRequest(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenWorkoutsExistOnOtherDaysOrForOtherUsers_ShouldNotCountTowardsLimit()
    {
        for (var i = 0; i < 5; i++)
        {
            _context.Workouts.Add(Workout(DateTime.UtcNow.AddDays(-1), $"Yesterday {i}"));
            _context.Workouts.Add(Workout(DateTime.UtcNow, $"Other {i}", OtherUserId));
        }
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, ValidRequest(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenExerciseDoesNotExist_ShouldReturnExerciseNotFound()
    {
        var result = await _handler.Handle(UserId, ValidRequest(BenchPressId, 999), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(ExerciseError.NotFound());
        (await _context.Workouts.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenExerciseIsDeleted_ShouldReturnExerciseNotFound()
    {
        var result = await _handler.Handle(UserId, ValidRequest(BenchPressId, DeletedExerciseId), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(ExerciseError.NotFound());
        (await _context.Workouts.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Handle_WhenSameExerciseIsUsedMultipleTimes_ShouldSucceed()
    {
        var result = await _handler.Handle(UserId, ValidRequest(BenchPressId, BenchPressId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Payload!.Exercises.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldPersistWorkoutWithEntriesAndSets()
    {
        var request = ValidRequest();
        request.ExerciseEntries.Add(new ExerciseEntryDto
        {
            ExerciseId = RunningId,
            Name = "Running",
            ExerciseType = ExerciseType.Cardio,
            Sets = [new SetEntryDto { Distance = 5.5m, DurationMinutes = 25, DurationSeconds = 30 }]
        });

        var result = await _handler.Handle(UserId, request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var saved = await _context.Workouts
            .Include(w => w.ExerciseEntries).ThenInclude(e => e.Sets)
            .SingleAsync();

        saved.OwnerId.Should().Be(UserId);
        saved.Name.Should().Be("Push Day");
        saved.Notes.Should().Be("Felt strong");
        saved.ExerciseEntries.Should().HaveCount(2);

        var bench = saved.ExerciseEntries.Single(e => e.ExerciseId == BenchPressId);
        bench.ExerciseType.Should().Be(ExerciseType.WeightLifting);
        bench.Sets.Should().ContainSingle(s => s.Reps == 8 && s.Weight == 80m);

        var running = saved.ExerciseEntries.Single(e => e.ExerciseId == RunningId);
        running.Sets.Single().DurationSeconds.Should().Be(25 * 60 + 30);
        running.Sets.Single().Distance.Should().Be(5.5m);
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldTruncateWorkoutDateToUtcDate()
    {
        var result = await _handler.Handle(UserId, ValidRequest(), CancellationToken.None);

        result.Payload!.WorkoutDate.Should().Be(Utc(2026, 5, 10));
        result.Payload.WorkoutDate.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldReturnMappedResponse()
    {
        var request = ValidRequest();
        request.ExerciseEntries[0] = request.ExerciseEntries[0] with
        {
            Sets = [new SetEntryDto { Reps = 1, Weight = 1m, DurationMinutes = 2, DurationSeconds = 15 }]
        };

        var result = await _handler.Handle(UserId, request, CancellationToken.None);

        var saved = await _context.Workouts.SingleAsync();
        var response = result.Payload!;
        response.Id.Should().Be(saved.Id).And.BeGreaterThan(0);
        response.UserId.Should().Be(UserId);
        response.Name.Should().Be("Push Day");
        response.Notes.Should().Be("Felt strong");

        var exercise = response.Exercises.Should().ContainSingle().Subject;
        exercise.ExerciseId.Should().Be(BenchPressId);
        exercise.ExerciseType.Should().Be(ExerciseType.WeightLifting);

        var set = exercise.Sets.Should().ContainSingle().Subject;
        set.Reps.Should().Be(1);
        set.Weight.Should().Be(1m);
        set.DurationMinutes.Should().Be(2);
        set.DurationSeconds.Should().Be(15);
    }

    [Fact]
    public async Task Handle_WhenSetHasNoDuration_ShouldStoreNullDuration()
    {
        var result = await _handler.Handle(UserId, ValidRequest(), CancellationToken.None);

        var set = result.Payload!.Exercises.Single().Sets.Single();
        set.DurationMinutes.Should().BeNull();
        set.DurationSeconds.Should().BeNull();
    }
}
