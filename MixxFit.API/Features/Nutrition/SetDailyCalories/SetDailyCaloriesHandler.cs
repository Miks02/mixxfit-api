using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Nutrition.SetDailyCalories;

public class SetDailyCaloriesHandler(AppDbContext context) : IHandler
{
    public async Task<Result<SetDailyCaloriesResponse>> Handle(
      string userId,
      SetDailyCaloriesRequest request,
      CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
            return Result<SetDailyCaloriesResponse>.Failure(UserError.NotFound(userId));

        user.DailyCalorieGoal = request.Calories;
        await context.SaveChangesAsync(cancellationToken);
        System.Console.WriteLine($"Set daily calories for user {userId} to {user.DailyCalorieGoal}");
        return Result<SetDailyCaloriesResponse>.Success(new SetDailyCaloriesResponse((double)user.DailyCalorieGoal));
    }
}
