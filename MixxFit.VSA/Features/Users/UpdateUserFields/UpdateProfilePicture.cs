using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;

namespace MixxFit.VSA.Features.Users.UpdateUserFields;

public static class UpdateProfilePicture
{
    public class UpdateProfilePictureHandler(UserManager<User> userManager, IFileService fileService) : IHandler
    {
        public async Task<Result<string?>> HandleUpdate(
            string userId,
            IFormFile imageFile,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<string?>.Failure(UserError.NotFound(userId));
            
            var uploadResult = await fileService.UploadFile(imageFile, user.ImagePath, "user_avatars");
            
            if(!uploadResult.IsSuccess)
                return Result<string?>.Failure(uploadResult.Errors.ToArray());

            user.ImagePath = uploadResult.Payload;

            var updateResult = await userManager.UpdateAsync(user);
            return updateResult.HandleIdentityResult(user.ImagePath);
        }
    }

    public class UpdateProfilePictureEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("users/profile-picture", async (
                IFormFile imageFile,
                ICurrentUserProvider currentUserProvider,
                UpdateProfilePictureHandler handler,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.HandleUpdate(currentUserProvider.GetCurrentUserId(), imageFile, cancellationToken);
                return result.ToTypedResult();
            })
            .WithTags("Users")
            .RequireAuthorization()
            .DisableAntiforgery()
            .Produces<string?>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
        }
    }
}
