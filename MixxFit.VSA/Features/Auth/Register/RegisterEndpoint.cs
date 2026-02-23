using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Auth.Register;

public class RegisterEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/register", async (RegisterRequest request, RegisterHandler handler, ICookieProvider cookieProvider) =>
        {
            var result = await handler.Handle(request);

            if(!result.IsSuccess)
                return Results.BadRequest(result.Errors[0]);
            
            cookieProvider.SetRefreshTokenCookie(result.Payload!.RefreshToken);
            return Results.Ok(result.Payload);
        })
        .WithTags("Auth")
        .Produces<RegisterResponse>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }
}