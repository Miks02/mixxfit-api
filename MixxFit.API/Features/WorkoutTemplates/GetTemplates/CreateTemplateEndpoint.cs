using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.WorkoutTemplates.GetTemplates;

public class CreateTemplateEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/workout-templates", async (
            GetTemplatesHandler handler, 
            ICurrentUserProvider userProvider, 
            CancellationToken ct = default) => await handler.Handle(userProvider.GetCurrentUserId(), ct))
            .RequireAuthorization()
            .WithTags("WorkoutTemplates")
            .Produces<GetTemplatesResponse>();
    }
}