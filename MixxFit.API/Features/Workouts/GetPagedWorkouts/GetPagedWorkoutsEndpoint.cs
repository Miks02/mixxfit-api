using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Workouts.GetPagedWorkouts;

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