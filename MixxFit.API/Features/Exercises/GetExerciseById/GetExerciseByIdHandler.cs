using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Exercises.GetExerciseById;

public class GetExerciseByIdHandler(AppDbContext context) : IHandler
{
    public async Task<GetExerciseByIdResponse?> Handle(string userId, int id, CancellationToken cancellationToken)
    {
        var exercise = await context.Exercises
            .Where(e => e.Id == id && e.FitnessProfile!.UserId == userId)
            .Select(e => new GetExerciseByIdResponse
            {
                Name = e.Name,
                CategoryId = e.ExerciseCategoryId,
                MuscleGroupId = e.MuscleGroupId,
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        return exercise;
    }
}