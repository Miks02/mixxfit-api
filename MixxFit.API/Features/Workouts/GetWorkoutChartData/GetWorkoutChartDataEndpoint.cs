using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Workouts.GetWorkoutChartData;

public class GetWorkoutChartDataEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workouts/workout-chart", async (
                [AsParameters] GetWorkoutChartDataRequest request, 
                GetWorkoutChartDataHandler handler,
                ICurrentUserProvider currentUserProvider,
                CancellationToken cancellationToken = default) =>
            {
                var workoutChart = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return TypedResults.Ok(workoutChart);
            })
            .WithTags("Workouts")
            .RequireAuthorization();
    }
}