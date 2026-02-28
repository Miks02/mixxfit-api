using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightLogs;

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