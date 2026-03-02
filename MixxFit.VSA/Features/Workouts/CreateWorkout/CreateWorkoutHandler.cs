using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.Workouts.CreateWorkout;

public class CreateWorkoutHandler(AppDbContext context) : IHandler
{
    public async Task<Result<CreateWorkoutResponse>> Handle(
        string userId,
        CreateWorkoutRequest request, 
        CancellationToken cancellationToken)
    {
        if (await IsWorkoutLimitReached(userId, cancellationToken))
            return Result<CreateWorkoutResponse>.Failure(GeneralError.LimitReached($"Workout limit has been reached for {userId}"));

        var newWorkout = BuildWorkout(request, userId);

        await context.AddAsync(newWorkout, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var workoutDto = ToWorkoutResponse(newWorkout);

        return Result<CreateWorkoutResponse>.Success(workoutDto);
    }
    
    private async Task<bool> IsWorkoutLimitReached(string userId, CancellationToken cancellationToken)
    {
        var workoutsToday = await context.Workouts
            .Where(w => w.UserId == userId && w.WorkoutDate.Date == DateTime.Today.ToUniversalTime())
            .Select(w => w.Id)
            .CountAsync(cancellationToken);
        
        if (workoutsToday == 5) return true;
        
        return false;
    }

    private static Workout BuildWorkout(CreateWorkoutRequest request, string userId)
    {
        return new Workout
        {
            Name = request.Name,
            Notes = request.Notes,
            UserId = userId,
            WorkoutDate = request.WorkoutDate.ToUniversalTime(),
            ExerciseEntries = request.ExerciseEntries.Select(e => new ExerciseEntry()
            {
                Name = e.Name,
                ExerciseType = e.ExerciseType,
                CardioType = e.CardioType,
                DistanceKm = e.DistanceKm,
                Duration = UtilityExtensions.ValidateMinutesAndSeconds(e.DurationMinutes, e.DurationSeconds),
                AvgHeartRate = e.AvgHeartRate,
                MaxHeartRate = e.MaxHeartRate,
                CaloriesBurned = e.CaloriesBurned,
                PaceMinPerKm = e.PaceMinPerKm,
                WorkIntervalSec = e.WorkIntervalSec,
                RestIntervalSec = e.RestIntervalSec,
                IntervalsCount = e.IntervalsCount,
                Sets = e.Sets.Select(s => new SetEntry()
                {
                    Reps = s.Reps,
                    WeightKg = s.WeightKg
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
            Exercises = workout.ExerciseEntries.Select(e => new ExerciseEntryDto()
            {
                Id = e.Id,
                Name = e.Name,
                ExerciseType = e.ExerciseType,
                CardioType = e.CardioType,
                AvgHeartRate = e.AvgHeartRate,
                CaloriesBurned = e.CaloriesBurned,
                DistanceKm = e.DistanceKm,
                DurationMinutes = e.Duration.ToIntegerFromNullableMinutes(),
                DurationSeconds = e.Duration.ToIntegerFromNullableSeconds(),
                Sets = e.Sets.Select(s => new SetEntryDto()
                {
                    Reps = s.Reps,
                    WeightKg = s.WeightKg
                }).ToList()
            }).ToList()
        };
    }
}