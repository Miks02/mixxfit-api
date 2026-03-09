using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.WeightEntries.GetWeightSummary;

public class GetWeightSummaryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("weight-entries", async (
                [AsParameters] GetWeightSummaryRequest request,
                ICurrentUserProvider userProvider,
                GetWeightSummaryHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var summary = await handler.Handle(userProvider.GetCurrentUserId(), request, cancellationToken);
                return TypedResults.Ok(summary);
            })
            .WithTags("WeightEntries")
            .RequireAuthorization();
    }
}