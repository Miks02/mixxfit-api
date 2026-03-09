using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Users.UpdateUserFields;

public static class UpdateGender
{
    public record UpdateGenderRequest(Gender Gender);
    public record UpdateGenderResponse(Gender? Gender);

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
        public async Task<Result<UpdateGenderResponse>> Handle(
            string userId,
            UpdateGenderRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<UpdateGenderResponse>.Failure(UserError.NotFound(userId));

            user.Gender = request.Gender;

            var updateResult = await userManager.UpdateAsync(user);

            return updateResult.HandleIdentityResult(new UpdateGenderResponse(user.Gender));
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
            .Produces<UpdateGenderResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
