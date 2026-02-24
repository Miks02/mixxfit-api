using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Workouts.GetWorkoutsOverview;

public class GetWorkoutsOverviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workouts/overview", async (
            [AsParameters] GetWorkoutsOverviewRequest request, 
            GetWorkoutsOverviewHandler handler,
            ICurrentUserProvider currentUserProvider,
            CancellationToken cancellationToken = default) =>
        {
            var workoutsOverview = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
            return TypedResults.Ok(workoutsOverview);
        })
        .WithTags("Workouts")
        .RequireAuthorization();
    }
}