using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;

namespace MixxFit.VSA.Features.Users.UpdateUserFields;

public static class UpdateEmail
{
    public record UpdateEmailRequest(string Email);

    public class UpdateEmailValidator : AbstractValidator<UpdateEmailRequest>
    {
        public UpdateEmailValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Enter a valid email address.");
        }
    }

    public class UpdateEmailHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result<string>> Handle(
            string userId,
            UpdateEmailRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<string>.Failure(UserError.NotFound(userId));
            

            user.Email = request.Email;

            var updateResult = await userManager.UpdateAsync(user);

            return updateResult.HandleIdentityResult(user.Email!);
        }
    }

    public class UpdateEmailEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/email", async (
                UpdateEmailRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateEmailHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization()
            .Produces<string>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
