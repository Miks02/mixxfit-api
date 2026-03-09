using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Domain.Entities;

namespace MixxFit.API.Features.Profile.UpdateFitnessProfile;

public static class UpdateDateOfBirth
{
    public record UpdateDateOfBirthRequest(DateTime DateOfBirth);

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

    public class UpdateDateOfBirthHandler(AppDbContext context) : IHandler
    {
        public async Task<Result<DateTime?>> Handle(
            string userId,
            UpdateDateOfBirthRequest request, CancellationToken cancellationToken = default)
        {
            var profile = await context.FitnessProfiles
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (profile is null)
                return Result<DateTime?>.Failure(UserError.NotFound(userId));

            profile.DateOfBirth = request.DateOfBirth.ToUniversalTime();

            await context.SaveChangesAsync(cancellationToken);

            return Result<DateTime?>.Success(profile.DateOfBirth);
        }
    }

    public class UpdateDateOfBirthEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("fitness-profile/date-of-birth", async (
                UpdateDateOfBirthRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateDateOfBirthHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("FitnessProfile")
            .RequireAuthorization()
            .Produces<DateTime?>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
