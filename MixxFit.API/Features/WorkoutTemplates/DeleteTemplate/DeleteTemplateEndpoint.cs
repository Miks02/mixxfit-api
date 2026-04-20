using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.WorkoutTemplates.DeleteTemplate;

public class DeleteTemplateEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/workout-templates/{id:int}", async (int id, DeleteTemplateHandler handler, ICurrentUserProvider userProvider, CancellationToken ct) =>
        {
            var result = await handler.Handle(userProvider.GetCurrentUserId(), id, ct);
            return result.ToTypedResult(HttpStatusCode.NoContent);
        })
        .WithTags("WorkoutTemplates")
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}