using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Users.UnsuspendUserAsAdmin;

public static class UnsuspendUserAsAdmin
{
    public class UnsuspendUserAsAdminHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result> Handle(string userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result.Failure(UserError.NotFound(userId));

            if (user.AccountStatus == AccountStatus.Active)
                return Result.Failure(UserError.UserAlreadyActive(userId));

            user.AccountStatus = AccountStatus.Active;

            var updateResult = (await userManager.UpdateAsync(user)).HandleIdentityResult();

            return updateResult;
        }
    }

    public class UnsuspendUserAsAdminEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("admin/users/{userId}/unsuspend", async (
                string userId,
                UnsuspendUserAsAdminHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(userId, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);
        }
    }
}
