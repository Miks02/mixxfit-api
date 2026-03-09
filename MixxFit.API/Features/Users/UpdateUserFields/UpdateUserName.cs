using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Users.UpdateUserFields;

public static class UpdateUserName
{
    public record UpdateUserNameRequest(string UserName);
    public record UpdateUserNameResponse(string UserName);

    public class UpdateUserNameValidator : AbstractValidator<UpdateUserNameRequest>
    {
        public UpdateUserNameValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MinimumLength(4)
                .WithMessage("At least 2 characters are required")
                .MaximumLength(20)
                .WithMessage("Username cannot exceed 20 characters");
        }
    }

    public class UpdateUserNameHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result<UpdateUserNameResponse>> Handle(
            string userId,
            UpdateUserNameRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<UpdateUserNameResponse>.Failure(UserError.NotFound(userId));

            user.UserName = request.UserName;

            var updateResult = await userManager.UpdateAsync(user);

            return updateResult.HandleIdentityResult(new UpdateUserNameResponse(user.UserName!));
        }
    }

    public class UpdateUserNameEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/username", async (
                UpdateUserNameRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateUserNameHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization()
            .Produces<UpdateUserNameResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
