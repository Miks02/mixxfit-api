using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Features.WeightEntries.Shared;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightChart;

public static class GetWeightChart
{
    public record Request(double? TargetWeight);
    
    public class Handler(AppDbContext context) : IHandler
    {
        public async Task<GetWeightChartResponse> Handle(
            string userId,
            double? targetWeight,
            CancellationToken cancellationToken)
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


            return new GetWeightChartResponse()
            {
                Entries = entries,
                TargetWeight = targetWeight
            };            
        }
    }
    
    public class Endpoint : IEndpoint {
        public void MapEndpoint(IEndpointRouteBuilder app) {
            app.MapGet("weight-entries/weight-chart", async (
                    double? targetWeight,
                    Handler handler,
                    ICurrentUserProvider userProvider,
                    CancellationToken cancellationToken = default) =>
            {
                return await handler.Handle(userProvider.GetCurrentUserId(), targetWeight, cancellationToken);
            })
            .WithTags("WeightEntries")
            .RequireAuthorization();
        }
    }
}