using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Auth.RotateTokens;

public class RotateTokensEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/refresh-token", async (
            RotateTokensHandler handler, 
            ICookieProvider cookieProvider,
            CancellationToken cancellationToken = default) =>
        {
            var request = new RotateTokensRequest(cookieProvider.GetRefreshTokenCookie());

            var result = await handler.Handle(request, cancellationToken);

            if (!result.IsSuccess)
            {
                cookieProvider.DeleteRefreshTokenCookie();
                return Results.BadRequest(result.Errors[0]);
            }
            
            cookieProvider.SetRefreshTokenCookie(result.Payload!.RefreshToken);
            return Results.Ok(result.Payload);
        })
        .WithTags("Auth")
        .Produces<RotateTokensResponse>()
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);
    }
}