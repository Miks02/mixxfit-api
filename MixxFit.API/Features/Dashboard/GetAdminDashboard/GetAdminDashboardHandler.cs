using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Dashboard.GetAdminDashboard;

public class GetAdminDashboardHandler(AppDbContext context) : IHandler
{
    public async Task<GetAdminDashboardResponse> Handle(CancellationToken cancellationToken = default)
    {
        return new GetAdminDashboardResponse
        {
            TotalUsers = await context.Users.CountAsync(cancellationToken),
            TotalExercises = await context.Exercises.CountAsync(cancellationToken),
            TotalWorkouts = await context.Workouts.CountAsync(cancellationToken),
            MostCommonExerciseType = await GetMostCommonExerciseTypeAsync(cancellationToken),
            TotalWeightEntries = await context.WeightEntries.CountAsync(cancellationToken),
            AverageUserAge = await GetAverageUserAgeAsync(cancellationToken)
        };
    }

    private async Task<ExerciseType?> GetMostCommonExerciseTypeAsync(CancellationToken cancellationToken)
    {
        return await context.ExerciseEntries
            .GroupBy(e => e.ExerciseType)
            .OrderByDescending(g => g.Count())
            .Select(g => (ExerciseType?)g.Key)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<double?> GetAverageUserAgeAsync(CancellationToken cancellationToken)
    {
        var birthDates = await context.FitnessProfiles
            .Where(fp => fp.DateOfBirth != null)
            .Select(fp => fp.DateOfBirth!.Value)
            .ToListAsync(cancellationToken);

        return birthDates.Count == 0
            ? null
            : birthDates.Average(dob => CalculateAge(dob));
    }

    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
            age--;
        return age;
    }
}
