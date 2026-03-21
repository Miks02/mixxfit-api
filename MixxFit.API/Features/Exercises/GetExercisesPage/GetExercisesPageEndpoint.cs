using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Exercises.GetExercisesPage;

public class GetExercisesPageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/exercises-page", async (
                    ICurrentUserProvider userProvider,
                    GetExercisesPageHandler handler,
                    CancellationToken cancellationToken = default) =>
                await handler.Handle(userProvider.GetCurrentUserId(), cancellationToken))
            .WithTags("Exercises")
            .RequireAuthorization()
            .Produces<GetExercisesPageResponse>();

    }
}