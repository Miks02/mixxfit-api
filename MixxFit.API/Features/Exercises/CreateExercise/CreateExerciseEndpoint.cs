using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Features.Exercises.Shared;

namespace MixxFit.API.Features.Exercises.CreateExercise;

public class CreateExerciseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/exercises", async (
            CreateExerciseRequest request, 
            ICurrentUserProvider userProvider, 
            CreateExerciseHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            var result = await handler.Handle(userProvider.GetCurrentUserId(), request, cancellationToken);
            return result.ToTypedResult(HttpStatusCode.Created);
        })
        .WithTags("Exercises")
        .RequireAuthorization()
        .Produces<ExerciseDto>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}