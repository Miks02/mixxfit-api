using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Dashboard.GetAdminDashboard;

public class GetAdminDashboardHandler(AppDbContext context) : IHandler
{
    public async Task<GetAdminDashboardResponse> Handle(GetAdminDashboardRequest request, CancellationToken cancellationToken = default)
    {
        return new GetAdminDashboardResponse
        {
            Users = await GetUsersPageAsync(request.Page, request.PageSize, cancellationToken),
            TotalExercises = await context.Exercises.CountAsync(cancellationToken),
            TotalWorkouts = await context.Workouts.CountAsync(cancellationToken),
            MostCommonExerciseType = await GetMostCommonExerciseTypeAsync(cancellationToken),
            TotalWeightEntries = await context.WeightEntries.CountAsync(cancellationToken),
            AverageUserAge = await GetAverageUserAgeAsync(cancellationToken)
        };
    }

    private async Task<PagedResult<AdminDashboardUserDto>> GetUsersPageAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var usersQuery = context.Users.OrderByDescending(u => u.CreatedAt);

        var totalCount = await usersQuery.CountAsync(cancellationToken);

        var projectedUsers = await usersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                Email = u.Email!,
                u.AccountStatus,
                u.CreatedAt,
                u.FitnessProfile.DateOfBirth,
                WorkoutCount = u.FitnessProfile.Workouts.Count,
                WeightEntryCount = u.FitnessProfile.WeightEntries.Count
            })
            .ToListAsync(cancellationToken);

        var users = projectedUsers
            .Select(u => new AdminDashboardUserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Age = u.DateOfBirth.HasValue ? CalculateAge(u.DateOfBirth.Value) : null,
                AccountStatus = u.AccountStatus,
                CreatedAt = u.CreatedAt,
                WorkoutCount = u.WorkoutCount,
                WeightEntryCount = u.WeightEntryCount
            })
            .ToList();

        return new PagedResult<AdminDashboardUserDto>(users, page, pageSize, totalCount, users.Count);
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
