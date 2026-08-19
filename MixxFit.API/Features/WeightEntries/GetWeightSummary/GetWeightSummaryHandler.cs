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
        CancellationToken ct)
    {
        var hasEntries = await context.WeightEntries
            .Where(w => w.FitnessProfile!.UserId == userId)
            .AnyAsync(ct);

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
        
        var lastWeightEntry = await context.WeightEntries
            .Where(w => w.FitnessProfile!.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new WeightRecordDto
            {
                Weight = w.Weight,
                CreatedAt = w.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        var weightDelta = lastWeightEntry is not null 
            ? await GetWeightDeltaAsync(userId, lastWeightEntry, ct) 
            : null;
        
        var weightListDetails = await GetWeightLogsAsync(userId, request.Month, request.Year, ct);
        var weightChart = await GetWeightChartAsync(userId, request.TargetWeight, ct);
        
        var yearMonths = await GetAvailableYearsAndMonthsAsync(userId, ct);
        
        return new GetWeightSummaryResponse
        {
            CurrentWeight = new CurrentWeightDto
            {
                Weight = lastWeightEntry?.Weight,
                CreatedAt = lastWeightEntry?.CreatedAt
            },
            WeightListDetails = weightListDetails,
            WeightChart = weightChart,
            WeightDelta = weightDelta,
            YearsAndMonthsGroup = yearMonths,
        };

    }
    
    private async Task<WeightListDetails> GetWeightLogsAsync(
        string userId,
        int? month = null,
        int? year = null,
        CancellationToken cancellationToken = default)
    {

        return new WeightListDetails
        {
            WeightLogs = await (await BuildWeightEntriesQuery(userId, month, year, cancellationToken)).ToListAsync(cancellationToken),
        };
    }
    
    private async Task<WeightChartDto> GetWeightChartAsync(
        string userId,
        double? targetWeight,
        CancellationToken cancellationToken = default)
    {

        var entries = await context.WeightEntries
            .Where(w => w.FitnessProfile!.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new WeightRecordDto
            {
                Id = w.Id,
                Weight = w.Weight,
                TimeLogged = w.Time,
                CreatedAt = w.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new WeightChartDto
        {
            Entries = entries,
            TargetWeight = targetWeight
        };

    }
    
    private async Task<int?> GetLastAvailableMonthByYear(string userId, int year, CancellationToken cancellationToken)
    {
        return await context.WeightEntries
            .Where(w => w.FitnessProfile!.UserId == userId && w.CreatedAt.Year == year)
            .MaxAsync(w => (int?)w.CreatedAt.Month, cancellationToken);
    }
    
    private async Task<IQueryable<WeightRecordDto>> BuildWeightEntriesQuery(
        string userId, 
        int? month = null, 
        int? year = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.WeightEntries
            .OrderByDescending(w => w.CreatedAt)
            .Where(w => w.FitnessProfile!.UserId == userId)
            .Select(w => new WeightRecordDto
            {
                Id = w.Id,
                Weight = w.Weight,
                TimeLogged = w.Time,
                CreatedAt = w.CreatedAt,
                Notes = w.Notes
            });

        year ??= DateTime.UtcNow.Year;

        month ??= await GetLastAvailableMonthByYear(userId, (int) year, cancellationToken);

        query = query.Where(w => w.CreatedAt.Year == year && w.CreatedAt.Month == month);
            
        return query;
    }

    private async Task<WeightDeltaDto?> GetWeightDeltaAsync(string userId, WeightRecordDto currentWeightData ,CancellationToken ct)
    {
        var deltaDto = await context.WeightEntries
            .Where(we => we.FitnessProfile!.UserId == userId && we.CreatedAt != currentWeightData.CreatedAt)
            .OrderByDescending(we => we.CreatedAt)
            .Select(we => new WeightDeltaDto
            {
                Delta = currentWeightData.Weight - we.Weight,
                CreatedAt = we.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (deltaDto is null)
            return null;
        
        if(currentWeightData.CreatedAt < deltaDto.CreatedAt)
            throw new ArgumentException($"Current weight data is older than than previous weight entry. Check the value passed as a parameter for {nameof(currentWeightData)}");
        
        return deltaDto;
    }

    private async Task<Dictionary<int, IEnumerable<int>>> GetAvailableYearsAndMonthsAsync(string userId,
        CancellationToken ct)
    {
        var yearsMonths = await context.WeightEntries
            .Where(we => we.FitnessProfile!.UserId == userId)
            .Select(we => new { we.CreatedAt.Year, we.CreatedAt.Month })
            .Distinct()
            .OrderByDescending(w => w.Year)
            .ThenByDescending(we => we.Month)
            .ToListAsync(ct);

        return yearsMonths
            .GroupBy(we => we.Year)
            .ToDictionary(
                group => group.Key,
                group => group.Select(we => we.Month));
    }
}