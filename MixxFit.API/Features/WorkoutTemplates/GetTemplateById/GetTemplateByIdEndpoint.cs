using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.WorkoutTemplates.GetTemplateById;

public class GetTemplateByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/workout-templates/{id:int}", async (int id, GetTemplateByIdHandler handler, ICurrentUserProvider userProvider, CancellationToken ct) =>
        {
            var result = await handler.Handle(userProvider.GetCurrentUserId(), id, ct);
            return result.ToTypedResult();
        })
        .WithTags("WorkoutTemplates")
        .RequireAuthorization()
        .Produces<GetTemplateByIdResponse>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}