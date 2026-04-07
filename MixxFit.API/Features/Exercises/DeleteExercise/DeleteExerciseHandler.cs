using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MixxFit.API.Features.Exercises.DeleteExercise;

public class DeleteExerciseHandler(AppDbContext context) : IHandler
{
    public async Task<Result> Handle(
        string userId,
        int id,
        CancellationToken cancellationToken)
    {
        var exercise = await context.Exercises
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, cancellationToken);

        if (exercise is null)
            return Result.Failure(GeneralError.NotFound("Exercise"));
        
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