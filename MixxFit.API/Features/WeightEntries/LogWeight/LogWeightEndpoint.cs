using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.WeightEntries.LogWeight;

public class LogWeightEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("weight-entries", async (
                [FromBody] LogWeightRequest request,
                LogWeightHandler handler,
                ICurrentUserProvider userProvider,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(userProvider.GetCurrentUserId(), request, cancellationToken);

                return result.ToTypedResult(HttpStatusCode.Created);
            })
            .WithTags("WeightEntries")
            .RequireAuthorization()
            .Produces<LogWeightResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status429TooManyRequests);
    }
}