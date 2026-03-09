using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Workouts.GetWorkoutChartData;

public class GetWorkoutChartDataHandler(AppDbContext context) : IHandler
{
    public async Task<GetWorkoutChartDataResponse> Handle(
        string userId, 
        GetWorkoutChartDataRequest request, 
        CancellationToken cancellationToken)
    {
        var years = await context.Workouts
            .Where(w => w.UserId == userId)
            .Select(w => w.WorkoutDate.Year)
            .Distinct()
            .OrderByDescending(w => w)
            .ToListAsync(cancellationToken);
        
        var selectedYear = request.Year ?? await GetLastWorkoutYearAsync(userId, cancellationToken);
        
        var stats = await context.Workouts
            .Where(w => w.UserId == userId && w.WorkoutDate.Year == selectedYear)
            .GroupBy(w => w.WorkoutDate.Month)
            .Select(g => new { Month = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return new GetWorkoutChartDataResponse()
        {
            Years = years,
            JanuaryWorkouts = stats.FirstOrDefault(x => x.Month == 1)?.Count ?? 0,
            FebruaryWorkouts = stats.FirstOrDefault(x => x.Month == 2)?.Count ?? 0,
            MarchWorkouts = stats.FirstOrDefault(x => x.Month == 3)?.Count ?? 0,
            AprilWorkouts = stats.FirstOrDefault(x => x.Month == 4)?.Count ?? 0,
            MayWorkouts = stats.FirstOrDefault(x => x.Month == 5)?.Count ?? 0,
            JuneWorkouts = stats.FirstOrDefault(x => x.Month == 6)?.Count ?? 0,
            JulyWorkouts = stats.FirstOrDefault(x => x.Month == 7)?.Count ?? 0,
            AugustWorkouts = stats.FirstOrDefault(x => x.Month == 8)?.Count ?? 0,
            SeptemberWorkouts = stats.FirstOrDefault(x => x.Month == 9)?.Count ?? 0,
            OctoberWorkouts = stats.FirstOrDefault(x => x.Month == 10)?.Count ?? 0,
            NovemberWorkouts = stats.FirstOrDefault(x => x.Month == 11)?.Count ?? 0,
            DecemberWorkouts = stats.FirstOrDefault(x => x.Month == 12)?.Count ?? 0
        };
    }
    
    private async Task<int?> GetLastWorkoutYearAsync(string userId, CancellationToken cancellationToken)
    {
        return await context.Workouts
            .Where(w => w.UserId == userId)
            .Select(w => (int?)w.WorkoutDate.Year) 
            .MaxAsync(cancellationToken);
    }
}