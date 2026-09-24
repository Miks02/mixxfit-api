using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Features.Users.DeleteUser;

namespace MixxFit.API.Features.Users.DeleteUserAsAdmin;

public static class DeleteUserAsAdmin
{
    public class DeleteUserAsAdminHandler(DeleteUserHandler deleteUserHandler) : IHandler
    {
        public Task<Result> Handle(string userId, CancellationToken cancellationToken = default)
            => deleteUserHandler.Handle(userId, cancellationToken);
    }

    public class DeleteUserAsAdminEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("admin/users/{userId}", async (
                string userId,
                DeleteUserAsAdminHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(userId, cancellationToken);
                return result.ToTypedResult(HttpStatusCode.NoContent);
            })
            .WithTags("Users")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);
        }
    }
}
