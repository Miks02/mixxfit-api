using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;

namespace MixxFit.API.Features.Users.GetPagedUsersForAdmin;

public class GetPagedUsersForAdminEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("admin/users", async (
                [AsParameters] GetPagedUsersForAdminRequest request,
                GetPagedUsersForAdminHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var users = await handler.Handle(request, cancellationToken);
                return TypedResults.Ok(users);
            })
            .WithTags("Users")
            .RequireAuthorization("AdminOnly")
            .Produces<PagedResult<GetPagedUsersForAdminResponse>>();
    }
}
