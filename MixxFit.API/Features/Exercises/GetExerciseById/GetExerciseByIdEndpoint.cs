using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Exercises.GetExerciseById;

public class GetExerciseByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/exercises/{id:int}", async (
            int id, 
            ICurrentUserProvider userProvider,
            GetExerciseByIdHandler handler, 
            CancellationToken cancellationToken) =>
        {
            var userId = userProvider.GetCurrentUserId();
            var exercise = await handler.Handle(userId, id, cancellationToken);
            
            return exercise is not null ? Results.Ok(exercise) : Results.NotFound();
        })
        .WithTags("Exercises")
        .RequireAuthorization()
        .Produces<GetExerciseByIdResponse>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}