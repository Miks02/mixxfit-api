using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;

namespace MixxFit.VSA.Features.Users.UpdateUserFields;

public static class UpdateFullName
{
    public record UpdateFullNameRequest(string FirstName, string LastName);
    public record UpdateFullNameResponse(string FirstName, string LastName);

    public class UpdateFullNameHandler(UserManager<User> userManager) : IHandler
    {
        public async Task<Result<UpdateFullNameResponse>> Handle(
            string userId,
            UpdateFullNameRequest request, CancellationToken cancellationToken = default)
        {
            
            var user = await GetUserForUpdateAsync(userId, cancellationToken);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            var updateResult = await userManager.UpdateAsync(user);
            
            if(!updateResult.Succeeded)
                return Result<UpdateFullNameResponse>.Failure(updateResult.Errors.ToArray());
            
            return Result<UpdateFullNameResponse>.Success(new UpdateFullNameResponse(user.FirstName, user.LastName));
        }
        
        private async Task<User> GetUserForUpdateAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "CRITICAL ERROR: UserID is null or empty");

            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            return user ?? throw new InvalidOperationException("CRITICAL ERROR: User is null");
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