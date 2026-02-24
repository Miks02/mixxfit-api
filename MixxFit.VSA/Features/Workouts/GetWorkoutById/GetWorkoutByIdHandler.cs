using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.Workouts.GetWorkoutById;

public class GetWorkoutByIdHandler(AppDbContext context) : IHandler
{
    public async Task<Result<GetWorkoutByIdResponse>> Handle(string userId, int workoutId, CancellationToken cancellationToken)
    {
        var workout = await context.Workouts
            .AsNoTracking()
            .Where(w => w.UserId == userId && w.Id == workoutId)
            .Select(ProjectToWorkoutResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (workout is null)
            return Result<GetWorkoutByIdResponse>.Failure(GeneralError.NotFound("Requested workout was not found"));
        
        return Result<GetWorkoutByIdResponse>.Success(workout);
    }

    private Expression<Func<Workout, GetWorkoutByIdResponse>> ProjectToWorkoutResponse()
    {
        return w => new GetWorkoutByIdResponse()
        {
            Id = w.Id,
            Name = w.Name,
            Notes = w.Notes,
            WorkoutDate = w.WorkoutDate,
            CreatedAt = w.CreatedAt,
            UserId = w.UserId,
            Exercises = w.ExerciseEntries.Select(e => new ExerciseEntryDto()
            {
                Id = e.Id,
                AvgHeartRate = e.AvgHeartRate,
                MaxHeartRate = e.MaxHeartRate,
                ExerciseType = e.ExerciseType,
                CardioType = e.CardioType,
                DistanceKm = e.DistanceKm,
                DurationMinutes = e.Duration.ToIntegerFromNullableMinutes(),
                DurationSeconds = e.Duration.ToIntegerFromNullableSeconds(),
                CaloriesBurned = e.CaloriesBurned,
                Sets = e.Sets.Select(s => new SetEntryDto()
                {
                    Reps = s.Reps,
                    WeightKg = s.WeightKg
                })
            })

        };
    }
}