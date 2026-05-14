using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Features.Exercises.Shared;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;

namespace MixxFit.API.Features.Exercises.DeleteExercise;

public class DeleteExerciseHandler(AppDbContext context) : IHandler
{
    public async Task<Result> Handle(
        string userId,
        int id,
        CancellationToken cancellationToken)
    {
        var exercise = await context.Exercises
            .Include(e => e.FitnessProfile)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (exercise is null)
            return Result.Failure(ExerciseError.NotFound("Exercise"));
        
        if (exercise.FitnessProfile?.UserId != userId)
            return Result.Failure(GeneralError.Forbidden("You are not allowed to delete this exercise"));
        
        if (await IsExerciseRelated(id))
        {
            exercise.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        context.Exercises.Remove(exercise);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<bool> IsExerciseRelated(int exerciseId)
    {
        return await context.Exercises
            .Where(e => e.Id == exerciseId)
            .AnyAsync(e => e.ExerciseEntries.Count > 0);
    }
}