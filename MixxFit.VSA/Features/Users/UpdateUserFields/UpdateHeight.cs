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

public static class UpdateHeight
{
    public record UpdateHeightRequest(double Height);
    public record UpdateHeightResponse(double? Height);

    public class UpdateHeightValidator : AbstractValidator<UpdateHeightRequest>
    {
        public UpdateHeightValidator()
        {
            RuleFor(r => r.Height)
                .InclusiveBetween(70, 250)
                .WithMessage("Height must be between 70 and 250 cm.");
        }
    }

    public class UpdateHeightHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result<UpdateHeightResponse>> Handle(
            string userId,
            UpdateHeightRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<UpdateHeightResponse>.Failure(UserError.NotFound(userId));

            user.HeightCm = request.Height;

            var updateResult = await userManager.UpdateAsync(user);

            return updateResult.HandleIdentityResult(new UpdateHeightResponse(user.HeightCm));
        }
    }

    public class UpdateHeightEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/height", async (
                UpdateHeightRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateHeightHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization()
            .Produces<UpdateHeightResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
