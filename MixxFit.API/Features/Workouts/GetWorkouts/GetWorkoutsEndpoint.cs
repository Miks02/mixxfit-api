using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Workouts.GetWorkouts;

public class GetWorkoutsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workouts", async (
            [AsParameters] GetWorkoutsRequest request,
            GetWorkoutsHandler handler,
            ICurrentUserProvider currentUserProvider,
            CancellationToken cancellationToken = default) =>
        {
            var workouts = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
            return TypedResults.Ok(workouts);
        })
        .WithTags("Workouts")
        .RequireAuthorization();
    }
}
