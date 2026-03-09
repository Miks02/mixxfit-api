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

public static class UpdateTargetWeight
{
    public record UpdateTargetWeightRequest(double TargetWeight);
    public record UpdateTargetWeightResponse(double TargetWeight);

    public class UpdateTargetWeightValidator : AbstractValidator<UpdateTargetWeightRequest>
    {
        public UpdateTargetWeightValidator()
        {
            RuleFor(r => r.TargetWeight)
                .InclusiveBetween(25, 400)
                .WithMessage("Target weight must be between 25 and 400 kg.");
        }
    }

    public class UpdateTargetWeightHandler(AppDbContext context) : IHandler
    {
        public async Task<Result<UpdateTargetWeightResponse>> Handle(
            string userId,
            UpdateTargetWeightRequest request, CancellationToken cancellationToken = default)
        {
            var profile = await context.FitnessProfiles
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (profile is null)
                return Result<UpdateTargetWeightResponse>.Failure(UserError.NotFound(userId));

            profile.TargetWeight = request.TargetWeight;

            await context.SaveChangesAsync(cancellationToken);

            return Result<UpdateTargetWeightResponse>.Success(new UpdateTargetWeightResponse((double)profile.TargetWeight));
        }
    }

    public class UpdateTargetWeightEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("fitness-profile/target-weight", async (
                UpdateTargetWeightRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateTargetWeightHandler handler,
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
