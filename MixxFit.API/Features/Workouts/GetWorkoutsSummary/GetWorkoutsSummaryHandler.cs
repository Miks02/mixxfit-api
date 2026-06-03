using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Workouts.GetWorkoutsSummary;

public class GetWorkoutsSummaryHandler(AppDbContext context) : IHandler
{
    public async Task<GetWorkoutsSummaryResponse> Handle(string userId, CancellationToken ct)
    {
        DateOnly lastWorkoutDate = await context.Workouts
            .AsNoTracking()
            .Where(w => w.FitnessProfile.UserId == userId)
            .MaxAsync(w => DateOnly.FromDateTime(w.WorkoutDate), ct);

        var workoutCount = await context.Workouts
            .Where(w => w.FitnessProfile.UserId == userId)
            .Select(w => w.Id)
            .CountAsync(ct);

        var exerciseCount = await context.Workouts
            .Where(w => w.FitnessProfile.UserId == userId)
            .SelectMany(w => w.ExerciseEntries)
            .Select(e => e.Id)
            .CountAsync(ct);

        var favoriteExerciseType = await context.Workouts
            .Where(w => w.FitnessProfile.UserId == userId)
            .SelectMany(w => w.ExerciseEntries)
            .GroupBy(e => e.ExerciseType)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync(ct);
        
        var mostActiveMonths = await context.Workouts
            .Where(w => w.FitnessProfile.UserId == userId)
            .GroupBy(w => new { w.WorkoutDate.Month, w.WorkoutDate.Year })
            .OrderByDescending(g => g.Count())
            .Select(w => new MostActiveMonthDto
            {
                Month = (Month)w.Key.Month,
                Year = w.Key.Year,
                WorkoutCount = w.Count()
            })
            .Take(4)
            .ToListAsync(ct);
        
        return new GetWorkoutsSummaryResponse
        {
            WorkoutCount = workoutCount,
            ExerciseCount = exerciseCount,
            LastWorkoutDate = lastWorkoutDate,
            FavoriteExerciseType = favoriteExerciseType,
            WorkoutStreak = await CalculateWorkoutStreakAsync(userId, ct),
            MostActiveMonths = mostActiveMonths
        };
    }
    
    private async Task<int> CalculateWorkoutStreakAsync(string userId, CancellationToken cancellationToken = default)
    {
        var workoutDates = await context.Workouts
            .Where(u => u.FitnessProfile!.UserId == userId)
            .Select(w => w.WorkoutDate.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync(cancellationToken);

        if (workoutDates.Count == 0)
            return 0;
        
        var today = DateTime.UtcNow.Date;
        var startDate = workoutDates[0] == today 
            ? today 
            : today.AddDays(-1);
        
        if(startDate > workoutDates[0])
            return 0;
        
        var streakDays = 0;
        
        foreach (var workout in workoutDates.Take(10))
        {
            
            if (workout == startDate.AddDays(-streakDays))
            {
                streakDays++;
                continue;
            }
            
            break;
        }

        return streakDays;
    }

}
