using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Domain.Entities.Workouts;

namespace MixxFit.API.Features.Workouts.GetWorkoutById;

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
            return Result<GetWorkoutByIdResponse>.Failure(WorkoutError.NotFound($"Workout with id '{workoutId}' was not found for user '{userId}'"));
        
        return Result<GetWorkoutByIdResponse>.Success(workout);
    }

    private Expression<Func<Workout, GetWorkoutByIdResponse>> ProjectToWorkoutResponse()
    {
        return w => new GetWorkoutByIdResponse
        {
            Id = w.Id,
            Name = w.Name,
            Notes = w.Notes,
            UserId = w.UserId,
            CreatedAt = w.CreatedAt,
            WorkoutDate = w.WorkoutDate,
            Exercises = w.ExerciseEntries.Select(e => new ExerciseEntryDto
            {
                Id = e.Id,
                Name = e.Name,
                ExerciseType = e.ExerciseType,
                Sets = e.Sets.Select(s => new SetEntryDto
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