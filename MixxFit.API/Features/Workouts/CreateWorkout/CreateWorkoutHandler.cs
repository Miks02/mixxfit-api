using CloudinaryDotNet.Core;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Exercises.Shared;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Workouts.CreateWorkout;

public class CreateWorkoutHandler(AppDbContext context, ILogger<CreateWorkoutHandler> logger) : IHandler
{
    public async Task<Result<CreateWorkoutResponse>> Handle(
        string userId,
        CreateWorkoutRequest request, 
        CancellationToken cancellationToken)
    {
        if (await IsWorkoutLimitReachedAsync(userId, cancellationToken))
            return Result<CreateWorkoutResponse>.Failure(GeneralError.LimitReached($"Workout limit has been reached for {userId}"));

        var inputIds = request.ExerciseEntries
            .Select(e => e.ExerciseId)
            .ToHashSet();
        
        if (!await AreExercisesValidAsync(inputIds, cancellationToken))
            return Result<CreateWorkoutResponse>.Failure(ExerciseError.NotFound());
        
        var newWorkout = BuildWorkout(request, userId);
        
        context.Add(newWorkout);
        await context.SaveChangesAsync(cancellationToken);
        
        var workoutDto = ToWorkoutResponse(newWorkout);
        
        return Result<CreateWorkoutResponse>.Success(workoutDto);
    }
    
    private async Task<bool> IsWorkoutLimitReachedAsync(string userId, CancellationToken cancellationToken)
    {
        var workoutsToday = await context.Workouts
            .Where(w => w.UserId == userId && w.WorkoutDate.Date == DateTime.Today.ToUniversalTime())
            .Select(w => w.Id)
            .CountAsync(cancellationToken);
        
        return workoutsToday == 5;
    }

    private async Task<bool> AreExercisesValidAsync(HashSet<int> inputIds, CancellationToken cancellationToken)
    {
        var validIds = await context.Exercises
            .Where(e => inputIds.Contains(e.Id))
            .Select(e => new
            {
                e.Id,
                e.IsDeleted
            })
            .ToListAsync(cancellationToken);

        var invalidIds = inputIds
            .Except(validIds.Select(e => e.Id))
            .ToList();

        if (invalidIds.Count > 0)
        {
            logger.LogWarning("Some of the requested exercises do not exist in the system. Invalid IDs: {InvalidExerciseIds}", string.Join(", ", invalidIds));
            return false;
        }

        var deletedExercises = validIds
            .Where(e => e.IsDeleted)
            .ToList();

        if (deletedExercises.Count > 0)
        {
            logger.LogWarning("Some of the requested exercises are marked as deleted. Deleted IDs: {DeletedExerciseIds}", string.Join(", ", deletedExercises.Select(e => e.Id)));
            return false;
        }

        return true;
    }

    private static Workout BuildWorkout(CreateWorkoutRequest request, string userId)
    {
        return new Workout
        {
            Name = request.Name,
            Notes = request.Notes,
            UserId = userId,
            WorkoutDate = request.WorkoutDate.ToUniversalTime(),
            ExerciseEntries = request.ExerciseEntries.Select(e => new ExerciseEntry
            {
                ExerciseId = e.ExerciseId,
                Name = e.Name,
                ExerciseType = e.ExerciseType,
                Sets = e.Sets.Select(s => new SetEntry
                {
                    Reps = s.Reps,
                    Weight = s.Weight,
                    DurationSeconds = s.DurationMinutes.ToTotalSeconds(s.DurationSeconds),
                    Distance = s.Distance
                }).ToList()
            }).ToList()
        };
    }

    private static CreateWorkoutResponse ToWorkoutResponse(Workout workout)
    {
        return new CreateWorkoutResponse
        {
            Id = workout.Id,
            Name = workout.Name,
            Notes = workout.Notes,
            UserId = workout.UserId,
            CreatedAt = workout.CreatedAt,
            WorkoutDate = workout.WorkoutDate,
            Exercises = workout.ExerciseEntries.Select(e => new ExerciseEntryDto
            {
                ExerciseId = e.ExerciseId,
                Name = e.Name,
                ExerciseType = e.ExerciseType,
                Sets = e.Sets.Select(s => new SetEntryDto()
                {
                    Reps = s.Reps,
                    Weight = s.Weight,
                    Distance = s.Distance,
                    DurationMinutes = s.DurationSeconds.ToMinutesFromSeconds(),
                    DurationSeconds = s.DurationSeconds.ToSecondsFromRemainderMinutes()
                }).ToList()
            }).ToList()
        };
    }
}