using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Exercises.DeleteExercise;

public class DeleteExerciseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/exercises/{id:int}", async (
            [FromQuery] int id,
            ICurrentUserProvider userProvider,
            DeleteExerciseHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(userProvider.GetCurrentUserId(), id, cancellationToken);
            return result.ToTypedResult(HttpStatusCode.NoContent);
        })
        .WithTags("Exercises")
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}