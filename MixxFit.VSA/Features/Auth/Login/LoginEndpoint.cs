using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Auth.Login;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", async (LoginRequest request, LoginHandler handler, ICookieProvider cookieProvider) =>
        {
            var result = await handler.Handle(request);

            if(!result.IsSuccess)
                return Results.BadRequest(result.Errors[0]);
            
            cookieProvider.SetRefreshTokenCookie(result.Payload!.RefreshToken);
            return Results.Ok(result.Payload);
        })
        .Produces<LoginResponse>()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}