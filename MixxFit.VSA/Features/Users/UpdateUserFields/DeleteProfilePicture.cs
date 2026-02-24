using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;

namespace MixxFit.VSA.Features.Users.UpdateUserFields;

public static class DeleteProfilePicture
{
    public class DeleteProfilePictureHandler(UserManager<User> userManager, IFileService fileService) : IHandler
    {
        public async Task<Result> HandleDelete(string userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result.Failure(UserError.NotFound(userId));

            if (string.IsNullOrEmpty(user.ImagePath))
                return Result.Failure(new Error("Resource.NotFound", "Profile image not found"));

            var deleteResult = fileService.DeleteFile(user.ImagePath);
            
            if (!deleteResult.IsSuccess)
                return Result.Failure(deleteResult.Errors.ToArray());
            
            user.ImagePath = null;
            var updateResult = await userManager.UpdateAsync(user);
            return updateResult.HandleIdentityResult();
        }
    }
    
    public class DeleteProfileEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("users/profile-picture", async (
                    ICurrentUserProvider currentUserProvider,
                    DeleteProfilePictureHandler handler,
                    CancellationToken cancellationToken = default) =>
                {
                    var result = await handler.HandleDelete(currentUserProvider.GetCurrentUserId(), cancellationToken);
                    return result.ToTypedResult();
                })
                .WithTags("Users")
                .RequireAuthorization()
                .Produces(StatusCodes.Status204NoContent)
                .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
                .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
        }
    }
}