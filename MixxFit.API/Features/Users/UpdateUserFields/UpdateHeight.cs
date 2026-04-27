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

    public class UpdateHeightHandler(AppDbContext context) : IHandler
    {
        public async Task<Result<UpdateHeightResponse>> Handle(
            string userId,
            UpdateHeightRequest request, CancellationToken cancellationToken = default)
        {
            var user = await context.FitnessProfiles
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<UpdateHeightResponse>.Failure(UserError.NotFound(userId));

            user.Height = request.Height;
            
            return Result<UpdateHeightResponse>.Success(new UpdateHeightResponse(user.Height));
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
