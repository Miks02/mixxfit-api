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
            .Select(u => new UserDetailsDto(
                FullName: u.FirstName + " " + u.LastName,
                UserName: u.UserName!,
                Email: u.Email!,
                ImagePath: u.ImagePath,
                CurrentWeight: u.CurrentWeight,
                TargetWeight: u.TargetWeight,
                Height: u.HeightCm,
                DailyCalorieGoal: u.DailyCalorieGoal,
                DateOfBirth: u.DateOfBirth,
                AccountStatus: u.AccountStatus,
                Gender: u.Gender
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<UserDetailsDto>.Failure(UserError.NotFound(userId));

        return Result<UserDetailsDto>.Success(user);
    }
}