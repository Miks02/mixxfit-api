using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Common;
using MixxFit.API.Domain.Entities.Users;

namespace MixxFit.API.Features.Users.GetMe;

public class GetMeHandler(UserManager<User> userManager) : IHandler
{
    public async Task<Result<UserDetailsDto>> Handle(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .Where(u => u.Id == userId)
            .Include(u => u.FitnessProfile)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<UserDetailsDto>.Failure(UserError.NotFound(userId));

        var roles = await userManager.GetRolesAsync(user);

        var userDetails = new UserDetailsDto
        {
            FullName = user.FirstName + " " + user.LastName,
            UserName = user.UserName!,
            Email = user.Email!,
            ImagePath = user.ImagePath,
            CurrentWeight = user.FitnessProfile.Weight,
            TargetWeight = user.FitnessProfile.TargetWeight,
            Height = user.FitnessProfile.Height,
            DailyCalorieGoal = user.FitnessProfile.DailyCalorieGoal,
            DateOfBirth = user.FitnessProfile.DateOfBirth,
            AccountStatus = user.AccountStatus,
            Gender = user.FitnessProfile.Gender,
            Roles = roles.ToList()
        };

        return Result<UserDetailsDto>.Success(userDetails);
    }
}