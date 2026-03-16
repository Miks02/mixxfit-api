using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Exercises.GetExercises;

public class GetExercisesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/exercises", async (
            [AsParameters] GetExercisesRequest request, 
            ICurrentUserProvider userProvider, 
            GetExercisesHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            var userId = userProvider.GetCurrentUserId();
            return await handler.Handle(userId, request, cancellationToken);
        })
        .WithTags("Exercises")
        .RequireAuthorization()
        .Produces<GetExercisesResponse>();
    }
}