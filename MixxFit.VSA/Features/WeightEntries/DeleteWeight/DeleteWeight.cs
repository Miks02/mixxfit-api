using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.ErrorCatalog;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.WeightEntries.DeleteWeight;


public class DeleteWeightHandler(AppDbContext context, ILogger<DeleteWeightHandler> logger) : IHandler
{
    public async Task<Result> Handle(string userId, int id, CancellationToken cancellationToken)
    {
        var entry = await context.WeightEntries
            .Where(w => w.Id == id && w.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (entry is null)
            return Result.Failure(GeneralError.NotFound("Weight entry"));

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            context.WeightEntries.Remove(entry);
            await context.SaveChangesAsync(cancellationToken);

            var user = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(UserError.NotFound(userId));
            }

            user.CurrentWeight = await GetLastWeightFromUser(user.Id, cancellationToken);
            
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
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => w.Weight)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastWeight is 0)
            return null;

        return lastWeight;
    }
}
