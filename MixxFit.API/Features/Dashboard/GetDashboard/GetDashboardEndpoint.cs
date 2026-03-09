using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Dashboard.GetDashboard;

public class GetDashboardEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("dashboard", async (
                GetDashboardHandler handler,
                ICurrentUserProvider userProvider,
                CancellationToken cancellationToken = default) =>
            {
                var dashboard = await handler.Handle(userProvider.GetCurrentUserId(), cancellationToken);
                return TypedResults.Ok(dashboard);
            })
            .WithTags("Dashboard")
            .RequireAuthorization();
    }
}