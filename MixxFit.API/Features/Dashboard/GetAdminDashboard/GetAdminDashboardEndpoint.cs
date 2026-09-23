using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Dashboard.GetAdminDashboard;

public class GetAdminDashboardEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("admin/dashboard", async (
                GetAdminDashboardHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var dashboard = await handler.Handle(cancellationToken);
                return TypedResults.Ok(dashboard);
            })
            .WithTags("Dashboard")
            .RequireAuthorization("AdminOnly")
            .Produces<GetAdminDashboardResponse>();
    }
}
