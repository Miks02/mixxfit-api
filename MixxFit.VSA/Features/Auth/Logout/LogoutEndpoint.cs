using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Auth.Logout;

public class LogoutEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/logout",
                async (LogoutHandler handler, ICookieProvider cookieProvider, CancellationToken cancellationToken = default) =>
                {
                    var request = new LogoutRequest(cookieProvider.GetRefreshTokenCookie());
                    
                    var result = await handler.Handle(request, cancellationToken);

                    cookieProvider.DeleteRefreshTokenCookie();
                    return result.ToTypedResult();
                })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);
    }
}