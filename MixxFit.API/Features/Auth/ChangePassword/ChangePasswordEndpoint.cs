using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Auth.ChangePassword;

public class ChangePasswordEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/password", async (
                ChangePasswordRequest request,
                ChangePasswordHandler handler,
                ICurrentUserProvider currentUserProvider,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult(HttpStatusCode.NoContent);
            })
            .WithTags("Auth")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}