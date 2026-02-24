using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Workouts.GetWorkoutById;

public class GetWorkoutByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("workouts/{id:int}", async (
                int id,
                GetWorkoutByIdHandler handler,
                ICurrentUserProvider currentUserProvider,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), id, cancellationToken);

                return result.ToTypedResult();
            })
            .WithTags("Workouts")
            .RequireAuthorization()
            .Produces<GetWorkoutByIdResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}