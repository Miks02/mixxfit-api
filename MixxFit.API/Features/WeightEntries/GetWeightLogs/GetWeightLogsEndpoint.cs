using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.WeightEntries.GetWeightLogs;

public class GetWeightLogsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("weight-entries/logs", async (
                [AsParameters] GetWeightLogsRequest request,
                GetWeightLogsHandler handler,
                ICurrentUserProvider userProvider,
                CancellationToken cancellationToken = default) =>
            {
                var weightLogs = await handler.Handle(userProvider.GetCurrentUserId(), request, cancellationToken);
                return TypedResults.Ok(weightLogs);
            })
            .WithTags("WeightEntries")
            .RequireAuthorization();
    }
}