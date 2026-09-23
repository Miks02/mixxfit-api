using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Auth.SendResetPasswordLink;

public class SendResetPasswordLinkEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/forgot-password", async (
                SendResetPasswordLinkRequest request,
                SendResetPasswordLinkHandler handler) =>
            {
                var result = await handler.Handle(request);
                return result.ToTypedResult(HttpStatusCode.NoContent);
            })
            .WithTags("Auth")
            .RequireRateLimiting("ForgotPasswordLimiter")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status429TooManyRequests);
    }
}
