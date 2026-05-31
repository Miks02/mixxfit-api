using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WeightEntries.LogWeight;

public class LogWeightHandler(AppDbContext context, ILogger<LogWeightHandler> logger) : IHandler
{
    public async Task<Result<LogWeightResponse>> Handle(string userId, LogWeightRequest request, CancellationToken cancellationToken)
    {
        var hasLoggedWeightToday = await context.WeightEntries
                .Where(w => w.FitnessProfile!.UserId == userId && w.CreatedAt.Date == DateTime.UtcNow.Date)
                .AnyAsync(cancellationToken);

            if (hasLoggedWeightToday)
                return Result<LogWeightResponse>.Failure(WeightEntryError.LimitReached("Only one weight entry can be logged per day"));

            var fitnessProfile = await context.FitnessProfiles
                .FirstAsync(fp => fp.UserId == userId, cancellationToken);

            var newEntry = new WeightEntry
            {
                Weight = request.Weight,
                Time = request.Time,
                FitnessProfileId = fitnessProfile.Id,
                Notes = request.Notes
            };

            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await context.WeightEntries.AddAsync(newEntry, cancellationToken);
                
                fitnessProfile.Weight = newEntry.Weight;

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
