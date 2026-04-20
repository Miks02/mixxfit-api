using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.WorkoutTemplates.EditTemplate;

public class EditTemplateEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/workout-templates", async (
                EditTemplateRequest request,
                EditTemplateHandler handler,
                ICurrentUserProvider userProvider,
                CancellationToken ct) =>
            {
                var result = await handler.Handle(userProvider.GetCurrentUserId(), request, ct);
                return result.ToTypedResult();
            })
            .WithTags("WorkoutTemplates")
            .RequireAuthorization()
            .Produces<EditTemplateResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

    }
}