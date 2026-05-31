using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.ExerciseEntries;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.SetEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Workouts.CreateWorkout;

public class CreateWorkoutHandler(AppDbContext context, ILogger<CreateWorkoutHandler> logger) : IHandler
{
    public async Task<Result<CreateWorkoutResponse>> Handle(
        string userId,
        CreateWorkoutRequest request, 
        CancellationToken ct)
    {
        var nullableFitnessProfileId = await context.FitnessProfiles
            .Where(fp => fp.UserId == userId)
            .Select(fp => (int?)fp.Id)
            .FirstOrDefaultAsync(ct);
        
        if(nullableFitnessProfileId is null)
            return Result<CreateWorkoutResponse>.Failure(FitnessProfileError.NotFound($"Fitness profile for user '{userId}' was not found"));
        
        int fitnessProfileId = (int)nullableFitnessProfileId;
        
        if (await IsWorkoutLimitReachedAsync(fitnessProfileId, ct))
            return Result<CreateWorkoutResponse>.Failure(WorkoutError.LimitReached($"Workout limit has been reached for user '{userId}'"));

        var inputIds = request.ExerciseEntries
            .Select(e => e.ExerciseId)
            .ToHashSet();
        
        if (!await AreExercisesValidAsync(inputIds, ct))
            return Result<CreateWorkoutResponse>.Failure(ExerciseError.NotFound());
        
        var newWorkout = BuildWorkout(request, fitnessProfileId);
        
        context.Add(newWorkout);
        await context.SaveChangesAsync(ct);
        
        var workoutDto = ToWorkoutResponse(newWorkout);
        
        return Result<CreateWorkoutResponse>.Success(workoutDto);
    }
    
    private async Task<bool> IsWorkoutLimitReachedAsync(int fitnessProfileId, CancellationToken cancellationToken)
    {
        var workoutsToday = await context.Workouts
            .Where(w => w.FitnessProfileId == fitnessProfileId && w.WorkoutDate.Date == DateTime.UtcNow.Date)
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

    private static Workout BuildWorkout(CreateWorkoutRequest request, int fitnessProfileId)
    {
        return new Workout
        {
            Name = request.Name,
            Notes = request.Notes,
            FitnessProfileId = fitnessProfileId,
            WorkoutDate = DateTime.SpecifyKind(request.WorkoutDate.Date, DateTimeKind.Utc),
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