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

public static class UpdateFullName
{
    public record UpdateFullNameRequest(string FirstName, string LastName);
    public record UpdateFullNameResponse(string FirstName, string LastName);

    public class UpdateFullNameValidator : AbstractValidator<UpdateFullNameRequest>
    {
        public UpdateFullNameValidator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .MinimumLength(2)
                .WithMessage("Minimum length for first name is 2 characters")
                .MaximumLength(20)
                .WithMessage("Maximum length for first name is 20 characters");
            
            RuleFor(r => r.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .MinimumLength(2)
                .WithMessage("Minimum length for last name is 2 characters.")
                .MaximumLength(20)
                .WithMessage("Maximum length for last name is 20 characters.");
        }
    }

    public class UpdateFullNameHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result<UpdateFullNameResponse>> Handle(
            string userId,
            UpdateFullNameRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if(user is null)
                return Result<UpdateFullNameResponse>.Failure(UserError.NotFound(userId));

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            var updateResult = await userManager.UpdateAsync(user);
            
            if(!updateResult.Succeeded)
                return Result<UpdateFullNameResponse>.Failure(updateResult.Errors.ToArray());
            
            return Result<UpdateFullNameResponse>.Success(new UpdateFullNameResponse(user.FirstName, user.LastName));
        }
    }

    public class UpdateFullNameEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/fullname", async (
                UpdateFullNameRequest request,
                ICurrentUserProvider currentUserProvider,
                UpdateFullNameHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                
                var result = await handler.Handle(currentUserProvider.GetCurrentUserId(), request, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization()
            .Produces<UpdateFullNameResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}