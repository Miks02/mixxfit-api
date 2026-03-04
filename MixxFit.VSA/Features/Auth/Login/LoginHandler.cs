using Microsoft.AspNetCore.Identity;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;
using MixxFit.VSA.Features.Common;

namespace MixxFit.VSA.Features.Auth.Login;

public class LoginHandler(UserManager<User> userManager, ITokenService tokenService) : IHandler
{
    public async Task<Result<LoginResponse>> Handle(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<LoginResponse>.Failure(AuthError.LoginFailed("Incorrect email or password"));
        
        if(!await userManager.CheckPasswordAsync(user, request.Password))
            return Result<LoginResponse>.Failure(AuthError.LoginFailed("Incorrect email or password"));
        
        var tokenResult = await tokenService.GenerateAuthTokens(user);
        
        if(!tokenResult.IsSuccess)
            return Result<LoginResponse>.Failure(tokenResult.Errors.ToArray());

        var userDetails = new UserDetailsDto(
            FullName: user.FirstName + " " + user.LastName,
            UserName: user.UserName!,
            Email: user.Email!,
            ImagePath: user.ImagePath,
            CurrentWeight: user.CurrentWeight,
            TargetWeight: user.TargetWeight,
            Height: user.HeightCm,
            DailyCalorieGoal: user.DailyCalorieGoal,
            DateOfBirth: user.DateOfBirth,
            AccountStatus: user.AccountStatus,
            Gender: user.Gender
        );
        
        var response = new LoginResponse(tokenResult.Payload!.AccessToken, tokenResult.Payload.RefreshToken, userDetails);
        
        return Result<LoginResponse>.Success(response);
    }
}