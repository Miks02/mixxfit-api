using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Features.WeightEntries.Shared;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WeightEntries.GetWeightLogs;

public class GetWeightLogsHandler(AppDbContext context) : IHandler
{
    public async Task<GetWeightLogsResponse> Handle(
        string userId,
        GetWeightLogsRequest request,
        CancellationToken ct = default)
    {
        var month = request.Month ?? await GetLastAvailableMonthByYear(userId, request.Year ?? DateTime.UtcNow.Year, ct);

        if (month is null)
        {
            return new GetWeightLogsResponse
            {
                WeightLogs = []
            };
        }
        
        var year = request.Year ?? DateTime.UtcNow.Year;
        
        return new GetWeightLogsResponse
        {
            WeightLogs = await BuildWeightEntriesQuery(userId, month.Value, year).ToListAsync(ct)
        };
    }
    
    private IQueryable<WeightRecordDto> BuildWeightEntriesQuery(
        string userId, 
        int month, 
        int year)
    {
        var startDate = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddMonths(1);
        
        var query = context.WeightEntries
            .OrderByDescending(w => w.CreatedAt)
            .Where(w => w.FitnessProfile!.UserId == userId && w.CreatedAt >= startDate && w.CreatedAt < endDate)
            .Select(w => new WeightRecordDto
            {
                Id = w.Id,
                Weight = w.Weight,
                TimeLogged = w.Time,
                CreatedAt = w.CreatedAt
            });
        
        return query;
    }
    
    private async Task<int?> GetLastAvailableMonthByYear(string userId, int year, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddYears(1);
        
        return await context.WeightEntries
            .Where(w => w.FitnessProfile!.UserId == userId && w.CreatedAt >= startDate && w.CreatedAt < endDate)
            .MaxAsync(w => (int?)w.CreatedAt.Month, cancellationToken);
    }
}