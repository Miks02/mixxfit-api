using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.WeightEntries.LogWeight;

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

                return result.ToTypedResult(HttpStatusCode.OK);
            })
            .WithTags("WeightEntries")
            .RequireAuthorization()
            .Produces<Created>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status429TooManyRequests);
    }
}