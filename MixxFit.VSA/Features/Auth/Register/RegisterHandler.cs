using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Features.Common;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.Auth.Register;

public class RegisterHandler(
    UserManager<User> userManager, 
    AppDbContext context,
    ILogger<RegisterHandler> logger,
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService) : IHandler
{
    public async Task<Result<RegisterResponse>> Handle(RegisterRequest request)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
        };
        
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var createResult = (await userManager.CreateAsync(user, request.Password)).HandleIdentityResult();

            if (!createResult.IsSuccess)
                return Result<RegisterResponse>.Failure(createResult.Errors.ToArray());

            var assignResult = await AssignRoleAsync(user);

            if (!assignResult.IsSuccess)
            {
                await transaction.RollbackAsync();
                return Result<RegisterResponse>.Failure(assignResult.Errors.ToArray());
            }
            
            var tokenResult = (await tokenService.GenerateAuthTokens(user)).HandleResult();
        
            if(!tokenResult.IsSuccess)
            {
                await transaction.RollbackAsync();
                return Result<RegisterResponse>.Failure(tokenResult.Errors.ToArray());
            }

            var fitnessProfile = CreateFitnessProfile(user.Id);
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            user.FitnessProfile = fitnessProfile;
            
            var userDetails = new UserDetailsDto(
                FullName: user.FirstName + " " + user.LastName,
                UserName: user.UserName,
                Email: user.Email,
                ImagePath: user.ImagePath,
                CurrentWeight: user.FitnessProfile.Weight,
                TargetWeight: user.FitnessProfile.TargetWeight,
                DailyCalorieGoal: user.FitnessProfile.DailyCalorieGoal,
                Height: user.FitnessProfile.Height,
                DateOfBirth: user.FitnessProfile.DateOfBirth,
                AccountStatus: user.AccountStatus,
                Gender: user.FitnessProfile.Gender
            );
            
            var response = new RegisterResponse(tokenResult.Payload!.AccessToken, tokenResult.Payload.RefreshToken, userDetails);
            
            return Result<RegisterResponse>.Success(response);
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Error occurred while creating user");
            await transaction.RollbackAsync();
            throw;
        }
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

    private FitnessProfile CreateFitnessProfile(string userId)
    {
        var fitnessProfile = new FitnessProfile
        {
            UserId = userId
        };
        
        context.FitnessProfiles.Add(fitnessProfile);
        return fitnessProfile;
    }
}