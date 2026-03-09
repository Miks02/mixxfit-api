using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Users.DeleteUser;

public class DeleteUserHandler(UserManager<User> userManager, IFileService fileService) : IHandler
{
    public async Task<Result> Handle(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Failure(UserError.NotFound(userId));

        var deleteResult = HandleFileDeletion(user.ImagePath);
        
        if(!deleteResult.IsSuccess)
            return deleteResult;
        
        var result = await userManager.DeleteAsync(user);
        return result.HandleIdentityResult();
    }
    
    private Result HandleFileDeletion(string? filePath)
    {
        if(string.IsNullOrWhiteSpace(filePath))
            return Result.Success();
        
        var deleteResult = fileService.DeleteFile(filePath);
        
        if(!deleteResult.IsSuccess)
            return Result.Failure(deleteResult.Errors.ToArray());
        
        return Result.Success();
    }
}
