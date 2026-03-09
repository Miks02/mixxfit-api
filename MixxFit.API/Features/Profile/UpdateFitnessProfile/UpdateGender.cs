using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Profile.UpdateFitnessProfile;

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

    public class UpdateGenderHandler(AppDbContext context) : IHandler
    {
        public async Task<Result<Gender?>> Handle(
            string userId,
            UpdateGenderRequest request, CancellationToken cancellationToken = default)
        {
            var profile = await context.FitnessProfiles
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (profile is null)
                return Result<Gender?>.Failure(UserError.NotFound(userId));

            profile.Gender = request.Gender;

            await context.SaveChangesAsync(cancellationToken);

            return Result<Gender?>.Success(profile.Gender);
        }
    }

    public class UpdateGenderEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("fitness-profile/gender", async (
                UpdateGenderRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateGenderHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("FitnessProfile")
            .RequireAuthorization()
            .Produces<Gender?>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
