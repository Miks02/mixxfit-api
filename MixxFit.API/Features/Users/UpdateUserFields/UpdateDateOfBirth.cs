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

public static class UpdateDateOfBirth
{
    public record UpdateDateOfBirthRequest(DateTime DateOfBirth);
    public record UpdateDateOfBirthResponse(DateTime? DateOfBirth);

    public class UpdateDateOfBirthValidator : AbstractValidator<UpdateDateOfBirthRequest>
    {
        public UpdateDateOfBirthValidator()
        {
            var today = DateTime.UtcNow.Date;

            RuleFor(x => x.DateOfBirth)
                .NotEqual(default(DateTime))
                .WithMessage("Date of birth is required")
                .LessThan(today)
                .WithMessage("Date of birth must be in the past")
                .GreaterThan(new DateTime(1900, 1, 1))
                .WithMessage("Date of birth year is invalid");
        }
    }

    public class UpdateDateOfBirthHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result<UpdateDateOfBirthResponse>> Handle(
            string userId,
            UpdateDateOfBirthRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<UpdateDateOfBirthResponse>.Failure(UserError.NotFound(userId));

            user.DateOfBirth = request.DateOfBirth.ToUniversalTime();

            var updateResult = await userManager.UpdateAsync(user);

            return updateResult.HandleIdentityResult(new UpdateDateOfBirthResponse(user.DateOfBirth));
        }
    }

    public class UpdateDateOfBirthEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/date-of-birth", async (
                UpdateDateOfBirthRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateDateOfBirthHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization()
            .Produces<UpdateDateOfBirthResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
