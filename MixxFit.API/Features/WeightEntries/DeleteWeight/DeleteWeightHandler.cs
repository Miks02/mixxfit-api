using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WeightEntries.DeleteWeight;


public class DeleteWeightHandler(AppDbContext context, ILogger<DeleteWeightHandler> logger) : IHandler
{
    public async Task<Result> Handle(string userId, int id, CancellationToken cancellationToken)
    {
        var entry = await context.WeightEntries
            .Where(w => w.Id == id && w.FitnessProfile!.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entry is null)
            return Result.Failure(GeneralError.NotFound("Weight entry"));

        var fitnessProfile = await context.FitnessProfiles
            .FirstOrDefaultAsync(fp => fp.UserId == userId, cancellationToken);
        
        if(fitnessProfile is null) 
            return Result.Failure(FitnessProfileError.NotFound(userId));

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            context.WeightEntries.Remove(entry);
            await context.SaveChangesAsync(cancellationToken);

            fitnessProfile.Weight = await GetLastWeightFromUser(userId, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("Transaction failed for user {id} with exception: {ex}. Rolling back changes...", userId, ex);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        return Result.Success();
    }

    private async Task<double?> GetLastWeightFromUser(string userId, CancellationToken cancellationToken)
    {
        var lastWeight = await context.WeightEntries
            .Where(w => w.FitnessProfile!.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => w.Weight)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastWeight is 0)
            return null;

        return lastWeight;
    }
}
