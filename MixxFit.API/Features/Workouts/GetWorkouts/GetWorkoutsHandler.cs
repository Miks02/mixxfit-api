using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Workouts.GetWorkouts;

public class GetWorkoutsHandler(AppDbContext context) : IHandler
{
    public async Task<GetWorkoutsResponse> Handle(
        string userId, 
        GetWorkoutsRequest request,
        CancellationToken cancellationToken)
    {
        var availableYears = await GetAvailableYearsAsync(userId, cancellationToken);
        int? targetYear = request.Year ?? (availableYears.Count > 0 ? availableYears.OrderByDescending(y => y).First() : null);
        var availableMonths = await GetAvailableMonthsAsync(userId, targetYear, cancellationToken);
        int? targetMonth = null;
        if (targetYear.HasValue)
            targetMonth = request.Month.HasValue
                ? (int)request.Month.Value
                : availableMonths.Count > 0 ? availableMonths.OrderByDescending(m => m).First()   : null;

        var workouts = await GetWorkoutListByParams(userId, request, targetYear, targetMonth, cancellationToken);

        return new GetWorkoutsResponse
        {
            Year = targetYear,
            Month = targetMonth,
            AvailableYears = availableYears,
            AvailableMonths = availableMonths,
            Workouts = workouts
        };
    }
    
    private async Task<IReadOnlyList<WorkoutListItemDto>> GetWorkoutListByParams(
        string userId,
        GetWorkoutsRequest request,
        int? targetYear,
        int? targetMonth,
        CancellationToken cancellationToken)
    {
        var query = context.Workouts
            .AsNoTracking()
            .Where(w => w.FitnessProfile!.UserId == userId);
        
        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(w => w.Name.Contains(request.Search));

        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            query = request.Sort switch
            {
                "newest" => query.OrderByDescending(w => w.WorkoutDate),
                "oldest" => query.OrderBy(w => w.WorkoutDate),
                _ => query.OrderByDescending(w => w.WorkoutDate)
            };
        }

        if (targetYear.HasValue)
            query = query.Where(w => w.WorkoutDate.Year == targetYear.Value);

        if (targetMonth.HasValue)
            query = query.Where(w => w.WorkoutDate.Month == targetMonth.Value);

        return await query
            .Select(w => new WorkoutListItemDto
            {
                Id = w.Id,
                Name = w.Name,
                WorkoutDate = w.WorkoutDate,
                ExerciseCount = w.ExerciseEntries.Count,
                SetCount = w.ExerciseEntries.Sum(e => e.Sets.Count),
                HasCardio = w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.Cardio),
                HasWeights = w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.WeightLifting),
                HasBodyWeight = w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.BodyWeight)
                
            })
            .ToListAsync(cancellationToken);
    }

    private async Task<IReadOnlyList<int>> GetAvailableYearsAsync(string userId, CancellationToken cancellationToken)
    {
        return await context.Workouts
            .Where(w => w.FitnessProfile!.UserId == userId)
            .Select(w => w.WorkoutDate.Year)
            .Distinct()
            .OrderBy(y => y)
            .ToListAsync(cancellationToken);
    }

    private async Task<IReadOnlyList<int>> GetAvailableMonthsAsync(string userId, int? year, CancellationToken cancellationToken)
    {
        if (!year.HasValue)
            return [];

        return await context.Workouts
            .Where(w => w.FitnessProfile!.UserId == userId && w.WorkoutDate.Year == year.Value)
            .Select(w => w.WorkoutDate.Month)
            .Distinct()
            .OrderBy(m => m)
            .ToListAsync(cancellationToken);
    }
}
