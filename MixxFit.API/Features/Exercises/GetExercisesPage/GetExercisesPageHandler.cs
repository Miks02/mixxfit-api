using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Features.Exercises.Shared;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Domain.Entities.FitnessProfiles;

namespace MixxFit.API.Features.Exercises.GetExercisesPage;

public class GetExercisesPageHandler(AppDbContext context) : IHandler
{
    public async Task<GetExercisesPageResponse> Handle(string userId, CancellationToken cancellationToken)
    {
        var fitnessProfile = await context.FitnessProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(fp => fp.UserId == userId, cancellationToken);

        if (fitnessProfile is null)
        {
            return new GetExercisesPageResponse
            {
                Exercises = [],
                MuscleGroups = [],
                ExerciseCategories = []
            };
        }
        
        var exercises = await context.Exercises
            .OrderBy(e => e.Name)
            .Where(e => e.FitnessProfileId == fitnessProfile.Id || e.FitnessProfileId == null)
            .Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name + $" ({e.ExerciseCategory.Name})",
                MuscleGroupName = e.MuscleGroup.Name,
                ExerciseCategoryName = e.ExerciseCategory.Name,
                ExerciseType = e.ExerciseType,
                IsUserDefined = e.FitnessProfileId == fitnessProfile.Id
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
