using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Features.WeightEntries.Shared;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WeightEntries.GetWeightSummary;

public class GetWeightSummaryHandler(AppDbContext context) : IHandler
{
    public async Task<GetWeightSummaryResponse> Handle(
        string userId, 
        GetWeightSummaryRequest request,
        CancellationToken cancellationToken)
    {
        var hasEntries = await context.WeightEntries
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .AnyAsync(cancellationToken);

        if (!hasEntries)
        {
            return new GetWeightSummaryResponse
            {
                WeightListDetails = new WeightListDetails
                {
                    WeightLogs = []
                },
                WeightChart = new WeightChartDto
                {
                    Entries = []
                }
            };
        }

        var firstWeightEntry = await context.WeightEntries
            .Where(w => w.UserId == userId)
            .Select(w => new WeightRecordDto()
            {
                Weight = w.Weight,
                CreatedAt = w.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        var lastWeightEntry = await context.WeightEntries
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new WeightRecordDto()
            {
                Weight = w.Weight,
                CreatedAt = w.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        
        var weightEntryYears = await GetUserWeightEntryYearsAsync(userId, cancellationToken);

        var weightListDetails = await GetWeightLogsAsync(userId, request.Month, request.Year, cancellationToken);

        var progress = lastWeightEntry!.Weight - firstWeightEntry!.Weight;

        var weightChart = await GetWeightChartAsync(userId, request.TargetWeight, cancellationToken);

        return new GetWeightSummaryResponse()
        {
            FirstEntry = firstWeightEntry,
            CurrentWeight = new CurrentWeightDto()
            {
                Weight = lastWeightEntry.Weight,
                CreatedAt = lastWeightEntry.CreatedAt
            },
            Progress = progress,
            Years = weightEntryYears,
            WeightListDetails = weightListDetails,
            WeightChart = weightChart
        };

    }
    
    private async Task<IReadOnlyList<int>> GetUserWeightEntryYearsAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        return await context.WeightEntries
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .Select(w => w.CreatedAt.Year)
            .Distinct()
            .OrderByDescending(w => w)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<WeightListDetails> GetWeightLogsAsync(
        string userId,
        int? month = null,
        int? year = null,
        CancellationToken cancellationToken = default)
    {

        return new WeightListDetails()
        {
            WeightLogs = await (await BuildWeightEntriesQuery(userId, month, year, cancellationToken)).ToListAsync(cancellationToken),
            Months = await GetUserWeightEntryMonthsByYearAsync(userId, year ?? DateTime.UtcNow.Year, cancellationToken)
        };
    }
    
    public async Task<WeightChartDto> GetWeightChartAsync(
        string userId,
        double? targetWeight,
        CancellationToken cancellationToken = default)
    {

        var entries = await context.WeightEntries
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new WeightRecordDto()
            {
                Id = w.Id,
                Weight = w.Weight,
                TimeLogged = w.Time,
                CreatedAt = w.CreatedAt
            })
            .ToListAsync(cancellationToken);


        return new WeightChartDto()
        {
            Entries = entries,
            TargetWeight = targetWeight
        };

    }
    
    private async Task<IReadOnlyList<int>> GetUserWeightEntryMonthsByYearAsync(
        string userId, 
        int year,
        CancellationToken cancellationToken)
    {
        return await context.WeightEntries
            .AsNoTracking()
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
    
    private async Task<IQueryable<WeightRecordDto>> BuildWeightEntriesQuery(
        string userId, 
        int? month = null, 
        int? year = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.WeightEntries
            .AsNoTracking()
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
}