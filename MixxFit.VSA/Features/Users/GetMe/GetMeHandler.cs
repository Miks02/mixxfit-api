using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Domain.ErrorCatalog;
using MixxFit.VSA.Features.Common;

namespace MixxFit.VSA.Features.Users.GetMe;

public class GetMeHandler(UserManager<User> userManager) : IHandler
{
    public async Task<Result<UserDetailsDto>> Handle(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .Where(u => u.Id == userId)
            .Include(u => u.FitnessProfile)
            .Select(u => new UserDetailsDto(
                FullName: u.FirstName + " " + u.LastName,
                UserName: u.UserName!,
                Email: u.Email!,
                ImagePath: u.ImagePath,
                CurrentWeight: u.FitnessProfile.Weight,
                TargetWeight: u.FitnessProfile.TargetWeight,
                Height: u.FitnessProfile.Height,
                DailyCalorieGoal: u.FitnessProfile.DailyCalorieGoal,
                DateOfBirth: u.FitnessProfile.DateOfBirth,
                AccountStatus: u.AccountStatus,
                Gender: u.FitnessProfile.Gender
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<UserDetailsDto>.Failure(UserError.NotFound(userId));

        return Result<UserDetailsDto>.Success(user);
    }
}