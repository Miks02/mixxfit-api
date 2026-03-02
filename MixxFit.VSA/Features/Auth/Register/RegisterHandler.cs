using Microsoft.AspNetCore.Identity;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Features.Common;

namespace MixxFit.VSA.Features.Auth.Register;

public class RegisterHandler(
    UserManager<User> userManager, 
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService) : IHandler
{
    public async Task<Result<RegisterResponse>> Handle(RegisterRequest request)
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
        
        var userDetails = new UserDetailsDto(
            FullName: user.FirstName + " " + user.LastName,
            UserName: user.UserName,
            Email: user.Email,
            ImagePath: user.ImagePath,
            CurrentWeight: user.CurrentWeight,
            TargetWeight: user.TargetWeight,
            Height: user.HeightCm,
            DateOfBirth: user.DateOfBirth,
            AccountStatus: user.AccountStatus,
            Gender: user.Gender
        );

        var response = new RegisterResponse(tokenResult.Payload!.AccessToken, tokenResult.Payload.RefreshToken, userDetails);
        
        return Result<RegisterResponse>.Success(response);
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