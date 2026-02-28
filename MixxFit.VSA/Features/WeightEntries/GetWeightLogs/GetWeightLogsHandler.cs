using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Features.WeightEntries.Shared;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightLogs;

public class GetWeightLogsHandler(AppDbContext context) : IHandler
{
    public async Task<GetWeightLogsResponse> Handle(
        string userId,
        GetWeightLogsRequest request,
        CancellationToken cancellationToken = default)
    {
        return new GetWeightLogsResponse
        {
            WeightLogs = await (await BuildWeightEntriesQuery(userId, request.Month, request.Year, cancellationToken)).ToListAsync(cancellationToken),
            Months = await GetUserWeightEntryMonthsByYearAsync(userId, request.Year ?? DateTime.UtcNow.Year, cancellationToken)
        };
    }
    
    private async Task<IQueryable<WeightRecordDto>> BuildWeightEntriesQuery(
        string userId, 
        int? month = null, 
        int? year = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.WeightEntries
            .OrderByDescending(w => w.CreatedAt)
            .Where(w => w.UserId == userId)
            .Select(w => new WeightRecordDto()
            {
                Id = w.Id,
                Weight = w.Weight,
                TimeLogged = w.Time,
                CreatedAt = w.CreatedAt
            });

        year ??= DateTime.UtcNow.Year;

        month ??= await GetLastAvailableMonthByYear(userId, (int) year, cancellationToken);

        query = query.Where(w => w.CreatedAt.Year == year && w.CreatedAt.Month == month);
            
        return query;
    }
    
    private async Task<IReadOnlyList<int>> GetUserWeightEntryMonthsByYearAsync(
        string userId, 
        int year,
        CancellationToken cancellationToken)
    {
        return await context.WeightEntries
            .Where(w => w.UserId == userId && w.CreatedAt.Year == year)
            .Select(w => w.CreatedAt.Month)
            .Distinct()
            .OrderByDescending(w => w)
            .ToListAsync(cancellationToken);
    }
    
    private async Task<int?> GetLastAvailableMonthByYear(string userId, int year, CancellationToken cancellationToken)
    {
        return await context.WeightEntries
            .AsNoTracking()
            .Where(w => w.UserId == userId && w.CreatedAt.Year == year)
            .MaxAsync(w => (int?)w.CreatedAt.Month, cancellationToken);
    }
}