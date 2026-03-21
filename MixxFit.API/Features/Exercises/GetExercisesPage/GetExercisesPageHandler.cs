using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Features.Exercises.Shared;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Exercises.GetExercisesPage;

public class GetExercisesPageHandler(AppDbContext context) : IHandler
{
    public async Task<GetExercisesPageResponse> Handle(string userId, CancellationToken cancellationToken)
    {
        var exercises = await context.Exercises
            .OrderBy(e => e.Name)
            .Where(e => e.UserId == userId || e.UserId == null)
            .Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name + $" ({e.ExerciseCategory.Name})",
                MuscleGroupName = e.MuscleGroup.Name,
                ExerciseCategoryName = e.ExerciseCategory.Name,
                ExerciseType = e.ExerciseType,
                IsUserDefined = e.UserId == userId
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
