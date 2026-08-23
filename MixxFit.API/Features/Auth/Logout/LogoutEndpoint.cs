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
                    var request = new LogoutRequest(cookieProvider.GetRefreshTokenCookie());
                    
                    var result = await handler.Handle(request);

                    cookieProvider.DeleteRefreshTokenCookie();
                    return result.ToTypedResult();
                })
            .WithTags("Auth")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);
    }
}