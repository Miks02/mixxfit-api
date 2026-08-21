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
        
        var currentWeight = await context.WeightEntries
            .Where(w => w.FitnessProfile!.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => new CurrentWeightDto
            {
                Weight = w.Weight,
                CreatedAt = w.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        var weightDelta = currentWeight is not null 
            ? await GetWeightDeltaAsync(userId, currentWeight, ct) 
            : null;
        
        var weightListDetails = await GetWeightLogsAsync(userId, request.Month, request.Year, ct);
        var weightChart = await GetWeightChartAsync(userId, request.TargetWeight, ct);
        
        var yearMonths = await GetAvailableYearsAndMonthsAsync(userId, ct);
        
        return new GetWeightSummaryResponse
        {
            CurrentWeight = currentWeight,
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
        var yearParam = year ?? DateTime.UtcNow.Year;
        var monthParam = month ?? await GetLastAvailableMonthByYear(userId, yearParam, cancellationToken);
        
        if (monthParam is null)
        {
            return new WeightListDetails
            {
                WeightLogs = []
            };
        }
        
        return new WeightListDetails
        {
            WeightLogs = await BuildWeightEntriesQuery(userId, monthParam.Value, yearParam).ToListAsync(cancellationToken),
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
        var startDate = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddYears(1);
        
        return await context.WeightEntries
            .Where(w => w.FitnessProfile!.UserId == userId && w.CreatedAt >= startDate && w.CreatedAt < endDate)
            .MaxAsync(w => (int?)w.CreatedAt.Month, cancellationToken);
    }
    
    private IQueryable<WeightRecordDto> BuildWeightEntriesQuery(
        string userId, 
        int month, 
        int year)
    {
        var query = context.WeightEntries
            .OrderByDescending(w => w.CreatedAt)
            .Where(w => w.FitnessProfile!.UserId == userId && w.CreatedAt.Year == year && w.CreatedAt.Month == month)
            .Select(w => new WeightRecordDto
            {
                Id = w.Id,
                Weight = w.Weight,
                TimeLogged = w.Time,
                CreatedAt = w.CreatedAt,
                Notes = w.Notes
            });
            
        return query;
    }

    private async Task<WeightDeltaDto?> GetWeightDeltaAsync(string userId, CurrentWeightDto currentWeightData ,CancellationToken ct)
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