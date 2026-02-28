using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.WeightEntries.LogWeight;

public class LogWeightHandler(AppDbContext context, ILogger<LogWeightHandler> logger) : IHandler
{
    public async Task<Result<LogWeightResponse>> Handle(string userId, LogWeightRequest request, CancellationToken cancellationToken)
    {
        var hasLoggedWeightToday = await context.WeightEntries
                .Where(w => w.UserId == userId && w.CreatedAt.Date == DateTime.UtcNow.Date)
                .Select(w => w.Id)
                .AnyAsync(cancellationToken);

            if (hasLoggedWeightToday)
                return Result<LogWeightResponse>.Failure(GeneralError.LimitReached());

            var newEntry = new WeightEntry()
            {
                Weight = request.Weight,
                Time = request.Time,
                UserId = userId,
                Notes = request.Notes
            };

            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await context.WeightEntries.AddAsync(newEntry, cancellationToken);

                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.Id == newEntry.UserId, cancellationToken);

                if (user is null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result<LogWeightResponse>.Failure(UserError.NotFound(userId));
                }

                user.CurrentWeight = newEntry.Weight;

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error happened while trying to commit the transaction");
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            
            var createdWeightEntry = new LogWeightResponse()
            {
                Id = newEntry.Id,
                Weight = newEntry.Weight,
                Time = newEntry.Time,
                Notes = newEntry.Notes,
                CreatedAt = newEntry.CreatedAt
            };

            return Result<LogWeightResponse>.Success(createdWeightEntry);
    }
}