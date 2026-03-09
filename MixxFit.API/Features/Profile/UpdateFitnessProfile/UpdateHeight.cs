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

public static class UpdateHeight
{
    public record UpdateHeightRequest(double Height);

    public class UpdateHeightValidator : AbstractValidator<UpdateHeightRequest>
    {
        public UpdateHeightValidator()
        {
            RuleFor(r => r.Height)
                .InclusiveBetween(70, 250)
                .WithMessage("Height must be between 70 and 250 cm.");
        }
    }

    public class UpdateHeightHandler(AppDbContext context) : IHandler
    {
        public async Task<Result<double?>> Handle(
            string userId,
            UpdateHeightRequest request, CancellationToken cancellationToken = default)
        {
            var profile = await context.FitnessProfiles
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (profile is null)
                return Result<double?>.Failure(UserError.NotFound(userId));

            profile.Height = request.Height;

            await context.SaveChangesAsync(cancellationToken);

            return Result<double?>.Success(profile.Height);
        }
    }

    public class UpdateHeightEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("fitness-profile/height", async (
                UpdateHeightRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateHeightHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("FitnessProfile")
            .RequireAuthorization()
            .Produces<double?>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
