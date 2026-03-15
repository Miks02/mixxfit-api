using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Exercises.GetExercisesPage;

public class GetExercisesPageHandler(AppDbContext context) : IHandler
{
    public async Task<GetExercisesPageResponse> Handle(string userId, CancellationToken cancellationToken)
    {
        var exercises = await context.Exercises
            .Where(e => e.UserId == userId || e.UserId == null)
            .Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                MuscleGroupName = e.MuscleGroup.Name,
                CategoryName = e.ExerciseCategory.Name,
                ExerciseType = e.ExerciseType,
                UserId = e.UserId
            })
            .ToListAsync(cancellationToken);
        
        var muscleGroups = await context.MuscleGroups
            .Select(e => new MuscleGroupDto
            {
                Name = e.Name,
                Id = e.Id
            })
            .ToListAsync(cancellationToken);
        
        var exerciseCategories = await context.ExerciseCategories
            .Select(e => new ExerciseCategoryDto
            {
                Name = e.Name,
                Id = e.Id
            })
            .ToListAsync(cancellationToken);
        
        return new GetExercisesPageResponse
        {
            Exercises = exercises,
            MuscleGroups = muscleGroups,
            ExerciseCategories = exerciseCategories
        };
    }
}