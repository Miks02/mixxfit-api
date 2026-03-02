using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.Enums;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.Workouts.GetWorkoutsOverview;

public class GetWorkoutsOverviewHandler(AppDbContext context) : IHandler
{
    public async Task<GetWorkoutsOverviewResponse> Handle(
        string userId,
        GetWorkoutsOverviewRequest request,
        CancellationToken cancellationToken)
    {
        var pagedQuery = BuildPagedWorkoutQuery(userId, request);

        var paginatedWorkouts = await pagedQuery.ToListAsync(cancellationToken);
        var totalPaginatedWorkouts = await pagedQuery.CountAsync(cancellationToken);
        var totalWorkouts = await CountWorkoutsAsync(userId, request, cancellationToken);
        
        var workoutSummary = await BuildWorkoutSummaryAsync(userId);
        var pagedResult = new PagedResult<WorkoutListItemDto>(paginatedWorkouts, request.Page, request.PageSize, totalWorkouts, totalPaginatedWorkouts);
        
        return new GetWorkoutsOverviewResponse(pagedResult, workoutSummary);
    }
    
    private IQueryable<Workout> BuildWorkoutQuery(string userId, GetWorkoutsOverviewRequest request)
    {
        var query = context.Workouts
            .AsNoTracking()
            .Include(w => w.ExerciseEntries)
            .Where(w => w.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(w => w.Name.Contains(request.Search));

        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            query = request.Sort switch
            {
                "newest" => query.OrderByDescending(w => w.WorkoutDate),
                "oldest" => query.OrderBy(w => w.WorkoutDate),
                _ => query
            };
        }

        if (request.Date is null) 
            return query;
        
        var startDate = request.Date.Value.Date;
        var endDate = startDate.AddDays(1);
        
        var utcStart = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
        
        return query.Where(w => w.WorkoutDate >= utcStart && w.WorkoutDate < utcEnd);
    }
    
    private IQueryable<WorkoutListItemDto> BuildPagedWorkoutQuery(string userId, GetWorkoutsOverviewRequest request)
    {
        var baseQuery = BuildWorkoutQuery(userId, request);

        var paged = baseQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        return paged.Select(w => new WorkoutListItemDto(
                Id: w.Id,
                Name: w.Name,
                WorkoutDate: w.WorkoutDate,
                ExerciseCount: w.ExerciseEntries.Count,
                SetCount: w.ExerciseEntries.Sum(e => e.Sets.Count),
                HasCardio:  w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.Cardio),
                HasWeights:  w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.WeightLifting),
                HasBodyWeight:  w.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.BodyWeight)
            ));
    }
    
    private async Task<int> CountWorkoutsAsync(
        string userId, 
        GetWorkoutsOverviewRequest request, 
        CancellationToken cancellationToken = default)
    {
        var baseQuery = BuildWorkoutQuery(userId, request);

        return await baseQuery
            .Select(w => w.Id)
            .CountAsync(cancellationToken);
    }
    
    private async Task<WorkoutSummaryDto> BuildWorkoutSummaryAsync(string userId)
    {
        DateTime? lastWorkoutDate = await context.Workouts
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .MaxAsync(w => (DateTime?)w.WorkoutDate);

        var workoutCount = await context.Workouts
            .Where(w => w.UserId == userId)
            .Select(w => w.Id)
            .CountAsync();

        var exerciseCount = await context.Workouts
            .Where(w => w.UserId == userId)
            .SelectMany(w => w.ExerciseEntries)
            .Select(e => e.Id)
            .CountAsync();

        var favoriteExerciseType = await context.Workouts
            .Where(w => w.UserId == userId)
            .SelectMany(w => w.ExerciseEntries)
            .GroupBy(e => e.ExerciseType)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync();

        return new WorkoutSummaryDto
        (
            WorkoutCount: workoutCount,
            ExerciseCount: exerciseCount,
            LastWorkoutDate: lastWorkoutDate,
            FavoriteExerciseType: favoriteExerciseType
        );
    }
}