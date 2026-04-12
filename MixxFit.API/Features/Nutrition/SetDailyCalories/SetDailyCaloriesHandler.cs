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
        var user = await context.FitnessProfiles.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        if (user is null)
            return Result<SetDailyCaloriesResponse>.Failure(UserError.NotFound(userId));

        user.DailyCalorieGoal = request.Calories;
        await context.SaveChangesAsync(cancellationToken);
        
        return Result<SetDailyCaloriesResponse>.Success(new SetDailyCaloriesResponse((double)user.DailyCalorieGoal));
    }
}
