using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Workouts.CreateWorkout;

public class CreateWorkoutEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("workouts", async (
                CreateWorkoutRequest request,
                ICurrentUserProvider userProvider,
                CreateWorkoutHandler handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(userProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult(HttpStatusCode.Created);
            })
            .WithTags("Workouts")
            .RequireAuthorization()
            .Produces<CreateWorkoutResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status429TooManyRequests);
    }
}
