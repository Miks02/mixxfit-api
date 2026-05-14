using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Domain.Entities.FitnessProfiles;

namespace MixxFit.API.Features.Exercises.GetExerciseById;

public class GetExerciseByIdHandler(AppDbContext context) : IHandler
{
    public async Task<GetExerciseByIdResponse?> Handle(string userId, int id, CancellationToken cancellationToken)
    {
        var exercise = await context.Exercises
            .Include(e => e.FitnessProfile)
            .Where(e => e.Id == id)
            .Select(e => new
            {
                Exercise = e,
                IsOwner = e.FitnessProfile != null && e.FitnessProfile.UserId == userId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (exercise is null || (exercise.Exercise.FitnessProfileId != null && !exercise.IsOwner))
        {
            return null;
        }
        
        return new GetExerciseByIdResponse
        {
            Name = exercise.Exercise.Name,
            CategoryId = exercise.Exercise.ExerciseCategoryId,
            MuscleGroupId = exercise.Exercise.MuscleGroupId,
        };
    }
}