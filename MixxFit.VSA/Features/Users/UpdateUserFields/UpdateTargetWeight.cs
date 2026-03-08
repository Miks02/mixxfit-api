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

    public class UpdateTargetWeightHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result<UpdateTargetWeightResponse>> Handle(
            string userId,
            UpdateTargetWeightRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<UpdateTargetWeightResponse>.Failure(UserError.NotFound(userId));

            user.TargetWeight = request.Weight;

            var updateResult = await userManager.UpdateAsync(user);

            return updateResult.HandleIdentityResult(new UpdateTargetWeightResponse(user.TargetWeight));
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
