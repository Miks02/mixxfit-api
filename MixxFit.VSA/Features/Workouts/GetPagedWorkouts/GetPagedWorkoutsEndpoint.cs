using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Workouts.GetPagedWorkouts;

public class GetPagedWorkoutsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workouts", async (
            [AsParameters] GetPagedWorkoutsRequest request,
            GetPagedWorkoutsHandler handler,
            ICurrentUserProvider currentUserProvider,
            CancellationToken cancellationToken = default) =>
        {
            var pagedWorkouts = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
            return TypedResults.Ok(pagedWorkouts);
        })
        .WithTags("Workouts")
        .RequireAuthorization();
    }
}