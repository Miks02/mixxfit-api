using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.WeightEntries.DeleteWeight;

public class DeleteWeightEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("weight-entries/{id:int}", async (
                int id,
                DeleteWeightHandler handler,
                ICurrentUserProvider userProvider,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(userProvider.GetCurrentUserId(), id, cancellationToken);

                return result.ToTypedResult(HttpStatusCode.NoContent);
            })
            .WithTags("WeightEntries")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}