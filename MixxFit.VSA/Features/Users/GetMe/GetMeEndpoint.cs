using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Features.Common;

namespace MixxFit.VSA.Features.Users.GetMe;

public class GetMeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/me", 
            async (GetMeHandler handler, ICurrentUserProvider currentUserProvider, CancellationToken cancellationToken = default) =>
        {
            var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), cancellationToken);

            return result.ToTypedResult();
        })
        .WithTags("Users")
        .RequireAuthorization()
        .Produces<UserDetailsDto>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}