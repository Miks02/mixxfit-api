using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.WorkoutTemplates.CreateTemplate;

public class CreateTemplateEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/workout-templates", async (
                CreateTemplateRequest request, 
                CreateTemplateHandler handler,
                ICurrentUserProvider userProvider,
                CancellationToken ct) =>
        {
            var result = await handler.Handle(userProvider.GetCurrentUserId(), request, ct);
            return result.ToTypedResult(HttpStatusCode.Created);
        })
        .WithTags("WorkoutTemplates")
        .RequireAuthorization()
        .Produces<CreateTemplateResponse>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status429TooManyRequests)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

    }
}