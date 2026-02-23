using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;

namespace MixxFit.VSA.Features.Auth.Register;

public class RegisterHandler(
    UserManager<User> userManager, 
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService) : IHandler
{
    public async Task<Result<RegisterResponse>> Handle(RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
        };
        
        var createResult = (await userManager.CreateAsync(user, request.Password)).HandleIdentityResult();

        if (!createResult.IsSuccess)
            return Result<RegisterResponse>.Failure(createResult.Errors.ToArray());
        
        var assignResult = await AssignRoleAsync(user);

        if (!assignResult.IsSuccess)
            return Result<RegisterResponse>.Failure(assignResult.Errors.ToArray());
        
        var tokenResult = (await tokenService.GenerateAuthTokens(user)).HandleResult();
        
        if(!tokenResult.IsSuccess)
            return Result<RegisterResponse>.Failure(tokenResult.Errors.ToArray());
        
        var userDetails = await GetUserDetailsAsync(user.Id, cancellationToken);

        var response = new RegisterResponse(tokenResult.Payload!.AccessToken, tokenResult.Payload.RefreshToken, userDetails);
        
        return Result<RegisterResponse>.Success(response);
    }
    
    private async Task<UserDetailsDto> GetUserDetailsAsync(string id, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserDetailsDto(
                FullName: u.FirstName + " " + u.LastName,
                Email: u.Email!,
                ImagePath: u.ImagePath,
                CurrentWeight: u.CurrentWeight,
                TargetWeight: u.TargetWeight,
                Height: u.HeightCm,
                DateOfBirth: u.DateOfBirth,
                AccountStatus: u.AccountStatus,
                Gender: u.Gender
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new InvalidOperationException("User not found");

        return user;
    }
    
    private async Task<Result> AssignRoleAsync(User user)
    {

        if (!await roleManager.RoleExistsAsync("User"))
        {
            var createRoleResult = (await roleManager.CreateAsync(new IdentityRole("User"))).HandleIdentityResult();

            if (!createRoleResult.IsSuccess)
                return createRoleResult;
        }

        var addToRoleResult = (await userManager.AddToRoleAsync(user, "User")).HandleIdentityResult();
        if (!addToRoleResult.IsSuccess)
            return addToRoleResult;

        return Result.Success();

    }
}