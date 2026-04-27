using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Domain.Entities.Users;

namespace MixxFit.API.Features.Users.UpdateUserFields;

public static class UpdateTargetWeight
{
    public record UpdateTargetWeightRequest(double Weight);
    public record UpdateTargetWeightResponse(double? Weight);

    public class UpdateTargetWeightValidator : AbstractValidator<UpdateTargetWeightRequest>
    {
        public UpdateTargetWeightValidator()
        {
            RuleFor(r => r.Weight)
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
            var user = await context.FitnessProfiles
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<UpdateTargetWeightResponse>.Failure(UserError.NotFound(userId));

            user.TargetWeight = request.Weight;
            
            return Result<UpdateTargetWeightResponse>.Success(new UpdateTargetWeightResponse(user.TargetWeight));
        }
    }

    public class UpdateTargetWeightEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/target-weight", async (
                UpdateTargetWeightRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateTargetWeightHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization()
            .Produces<UpdateTargetWeightResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
