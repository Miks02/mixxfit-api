using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Exercises.GetExercises;

public class GetExercisesHandler(AppDbContext context) : IHandler 
{
    public async Task<GetExercisesResponse> Handle(string userId, GetExercisesRequest request, CancellationToken cancellationToken)
    {
        var query = context.Exercises.AsQueryable();
        
        if(request.CategoryId != null)
            query = query.Where(e => e.ExerciseCategoryId == request.CategoryId);
        
        if(request.MuscleGroupId is not null)
            query = query.Where(e => e.MuscleGroupId == request.MuscleGroupId);
        
        query = request.OnlyUserDefined 
            ? query.Where(e => e.UserId == userId) 
            : query.Where(e => e.UserId == userId || e.UserId == null);
        
        if(!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(e => e.Name.Contains(request.SearchTerm));
        
        var exercises = await query
            .OrderBy(e => e.Name)
            .Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name + $" ({e.ExerciseCategory.Name})",
                MuscleGroupName = e.MuscleGroup.Name,
                ExerciseType = e.ExerciseType,
                IsUserDefined = e.UserId == userId
            })
            .ToListAsync(cancellationToken);
        
        return new GetExercisesResponse { Exercises = exercises };
    }
}