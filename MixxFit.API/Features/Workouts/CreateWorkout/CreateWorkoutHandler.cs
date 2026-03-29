using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Workouts.CreateWorkout;

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

        context.Add(newWorkout);
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
            ExerciseEntries = request.ExerciseEntries.Select(e => new ExerciseEntry
            {
                Name = e.Name,
                ExerciseType = e.ExerciseType,
                Sets = e.Sets.Select(s => new SetEntry
                {
                    Reps = s.Reps,
                    Weight = s.Weight,
                    DurationSeconds = UtilityExtensions.ValidateMinutesAndSeconds(s.DurationMinutes, s.DurationSeconds),
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
                Id = e.Id,
                Name = e.Name,
                ExerciseType = e.ExerciseType,
                Sets = e.Sets.Select(s => new SetEntryDto()
                {
                    Reps = s.Reps,
                    Weight = s.Weight,
                    Distance = s.Distance,
                    DurationMinutes = 0,
                    DurationSeconds = 0
                }).ToList()
            }).ToList()
        };
    }
}