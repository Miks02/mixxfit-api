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
        app.MapPost("auth/register", async (RegisterRequest request, RegisterHandler handler, CancellationToken cancellationToken = default) =>
        {
            var result = await handler.Handle(request, cancellationToken);

            return result.ToTypedResult(null, HttpStatusCode.Created);
        })
        .Produces<RegisterResponse>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<Error>(StatusCodes.Status400BadRequest);
    }
}