using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Dashboard.GetDashboard;

public class GetDashboardHandler(AppDbContext context) : IHandler
{
    public async Task<GetDashboardResponse> Handle(string userId, CancellationToken cancellationToken = default)
    {

        var lastWorkoutDate = await GetLastWorkoutAsync(userId, cancellationToken);
        var recentWorkouts = await GetRecentWorkoutsAsync(userId, 10, cancellationToken);


        return new GetDashboardResponse
        {
            LastWorkoutDate = lastWorkoutDate,
            RecentWorkouts = recentWorkouts
        };
    }
    
    private async Task<DateTime?> GetLastWorkoutAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Workouts
            .OrderByDescending(w => w.WorkoutDate)
            .Where(w => w.UserId == userId)
            .Select(w => (DateTime?)w.WorkoutDate)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    private async Task<IReadOnlyList<RecentWorkoutDto>> GetRecentWorkoutsAsync(string userId, int itemsToTake, CancellationToken cancellationToken = default)
    {
        if (itemsToTake <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(itemsToTake), "Items to take must be greater than zero");
        }

        return await context.Workouts
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.WorkoutDate)
            .Take(itemsToTake)
            .Select(w => new RecentWorkoutDto()
            {
                Id = w.Id,
                Name = w.Name,
                ExerciseCount = w.ExerciseEntries.Count,
                SetCount = w.ExerciseEntries.Sum(e => e.Sets.Count),
                WorkoutDate = w.WorkoutDate,
                HasCardio = w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.Cardio),
                HasWeights = w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.WeightLifting),
                HasBodyWeight = w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.BodyWeight)
            })
            .ToListAsync(cancellationToken);
    }
}