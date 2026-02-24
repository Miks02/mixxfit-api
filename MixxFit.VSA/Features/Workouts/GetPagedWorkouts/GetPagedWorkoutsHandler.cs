using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.Enums;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.Workouts.GetPagedWorkouts;

public class GetPagedWorkoutsHandler(AppDbContext context) : IHandler
{

    public async Task<PagedResult<GetPagedWorkoutsResponse>> Handle(
        string userId, 
        GetPagedWorkoutsRequest request,
        CancellationToken cancellationToken)
    {
        var pagedQuery = BuildPagedWorkoutQuery(userId, request);

        var paginatedWorkouts = await pagedQuery.ToListAsync(cancellationToken);
        var totalPaginatedWorkouts = await pagedQuery.CountAsync(cancellationToken);
        var totalWorkouts = await CountWorkoutsAsync(userId, request, cancellationToken);
        
        return new PagedResult<GetPagedWorkoutsResponse>(paginatedWorkouts, request.Page, request.PageSize, totalPaginatedWorkouts, totalWorkouts);
    }
    
    private IQueryable<Workout> BuildWorkoutQuery(string userId, GetPagedWorkoutsRequest request)
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

        if (request.Date is not null)
        {
            var startDate = request.Date.Value.Date;
            var endDate = startDate.AddDays(1);
            
            var utcStart = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            var utcEnd = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
            
            query = query.Where(w => w.WorkoutDate >= utcStart && w.WorkoutDate < utcEnd);
        }

        return query;
    }
    
    private IQueryable<GetPagedWorkoutsResponse> BuildPagedWorkoutQuery(string userId, GetPagedWorkoutsRequest request)
    {
        var baseQuery = BuildWorkoutQuery(userId, request);

        var paged = baseQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        return paged.Select(w => new GetPagedWorkoutsResponse(
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
        GetPagedWorkoutsRequest request, 
        CancellationToken cancellationToken = default)
    {
        var baseQuery = BuildWorkoutQuery(userId, request);

        return await baseQuery
            .Select(w => w.Id)
            .CountAsync(cancellationToken);
    }
    
    
}