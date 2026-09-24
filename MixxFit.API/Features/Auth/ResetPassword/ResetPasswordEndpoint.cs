using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Auth.ResetPassword;

public class ResetPasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/reset-password", async (
                ResetPasswordRequest request,
                ResetPasswordHandler handler,
                ICookieProvider cookieProvider) =>
            {
                var result = await handler.Handle(request);
                if (result.IsSuccess)
                    cookieProvider.DeleteRefreshTokenCookie();

                return result.ToTypedResult(HttpStatusCode.NoContent);
            })
            .WithTags("Auth")
            .RequireRateLimiting("AuthLimiter")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status429TooManyRequests);
    }
}
