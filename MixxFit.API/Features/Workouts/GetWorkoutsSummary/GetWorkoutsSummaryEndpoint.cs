


using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Workouts.GetWorkoutsSummary;

public class GetWorkoutSummaryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/workouts/summary", async
            (ICurrentUserProvider userProvider, GetWorkoutsSummaryHandler handler, CancellationToken ct) =>
            await handler.Handle(userProvider.GetCurrentUserId(), ct))
        .WithTags("Workouts")
        .RequireAuthorization();
    }
}
