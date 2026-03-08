using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        var user = await userManager.Users
            .Include(u => u.FitnessProfile)
            .Where(u => u.Email == request.Email)
            .FirstOrDefaultAsync();

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
            CurrentWeight: user.FitnessProfile.Weight,
            TargetWeight: user.FitnessProfile.TargetWeight,
            Height: user.FitnessProfile.Height,
            DailyCalorieGoal: user.FitnessProfile.DailyCalorieGoal,
            DateOfBirth: user.FitnessProfile.DateOfBirth,
            AccountStatus: user.AccountStatus,
            Gender: user.FitnessProfile.Gender
        );
        
        var response = new LoginResponse(tokenResult.Payload!.AccessToken, tokenResult.Payload.RefreshToken, userDetails);
        
        return Result<LoginResponse>.Success(response);
    }
}