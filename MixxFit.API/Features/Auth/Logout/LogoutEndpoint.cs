using MixxFit.API.Common.Interfaces;

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