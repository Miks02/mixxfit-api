using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Users.DeleteUser;

public class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("users", async (
            ICurrentUserProvider currentUserProvider,
            ICookieProvider cookieProvider,
            DeleteUserHandler handler,
            CancellationToken cancellationToken = default) =>
        {
            var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), cancellationToken);
            if (result.IsSuccess)
                cookieProvider.DeleteRefreshTokenCookie();

            return result.ToTypedResult(HttpStatusCode.NoContent);
        })
        .WithTags("Users")
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict);
    }
}
