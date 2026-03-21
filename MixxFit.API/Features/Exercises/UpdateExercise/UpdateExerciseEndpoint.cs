using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Features.Exercises.Shared;

namespace MixxFit.API.Features.Exercises.UpdateExercise;

public class UpdateExerciseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/exercises", async (
            UpdateExerciseRequest request, 
            ICurrentUserProvider userProvider, 
            UpdateExerciseHandler handler, 
            CancellationToken cancellationToken = default) =>
        {
            var userId = userProvider.GetCurrentUserId();
            var result = await handler.Handle(userId, request, cancellationToken);
            return result.ToTypedResult();
        })
        .WithTags("Exercises")
        .RequireAuthorization()
        .Produces<ExerciseDto>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}