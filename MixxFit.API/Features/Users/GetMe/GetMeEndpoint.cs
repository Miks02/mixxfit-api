using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Features.Common;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Users.GetMe;

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