using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.Enums;
using MixxFit.VSA.Domain.ErrorCatalog;

namespace MixxFit.VSA.Features.Users.UpdateUserFields;

public static class UpdateGender
{
    public record UpdateGenderRequest(Gender Gender);

    public class UpdateGenderValidator : AbstractValidator<UpdateGenderRequest>
    {
        public UpdateGenderValidator()
        {
            RuleFor(x => x.Gender)
                .IsInEnum()
                .NotEmpty()
                .WithMessage("Gender is required");
        }
    }

    public class UpdateGenderHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result<Gender?>> Handle(
            string userId,
            UpdateGenderRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<Gender?>.Failure(UserError.NotFound(userId));

            user.Gender = request.Gender;

            var updateResult = await userManager.UpdateAsync(user);

            return updateResult.HandleIdentityResult(user.Gender);
        }
    }

    public class UpdateGenderEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/gender", async (
                UpdateGenderRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateGenderHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization()
            .Produces<Gender?>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
