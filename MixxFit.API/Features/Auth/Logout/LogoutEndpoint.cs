using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Auth.Logout;

public class LogoutEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/logout",
                async (LogoutHandler handler, ICookieProvider cookieProvider) =>
                {
                    var refreshToken = cookieProvider.GetRefreshTokenCookie();

                    await handler.Handle(refreshToken);

                    cookieProvider.DeleteRefreshTokenCookie();
                    return TypedResults.NoContent();
                })
            .WithTags("Auth")
            .Produces(StatusCodes.Status204NoContent);
    }
}