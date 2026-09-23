using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Users.GetPagedUsersForAdmin;

public class GetPagedUsersForAdminHandler(AppDbContext context) : IHandler
{
    public async Task<PagedResult<GetPagedUsersForAdminResponse>> Handle(
        GetPagedUsersForAdminRequest request,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = context.Users.AsNoTracking();

        if (request.IsDeleted.HasValue)
            query = request.IsDeleted.Value
                ? query.Where(u => u.DeletedAt != null)
                : query.Where(u => u.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var escaped = request.Search.Trim()
                .Replace("\\", "\\\\")
                .Replace("%", "\\%")
                .Replace("_", "\\_");
            var pattern = $"%{escaped}%";

            query = query.Where(u =>
                EF.Functions.ILike(u.Email!, pattern) ||
                EF.Functions.ILike(u.UserName!, pattern) ||
                EF.Functions.ILike(u.FirstName + " " + u.LastName, pattern));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var projectedUsers = await ApplySort(query, request.Sort)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new
            {
                u.Id,
                FullName = GetFullName(u),
                Email = u.Email!,
                u.AccountStatus,
                u.CreatedAt,
                u.FitnessProfile.DateOfBirth,
                u.DeletedAt,
                WorkoutCount = u.FitnessProfile.Workouts.Count,
                WeightEntryCount = u.FitnessProfile.WeightEntries.Count
            })
            .ToListAsync(cancellationToken);

        var users = projectedUsers
            .Select(u => new GetPagedUsersForAdminResponse
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Age = u.DateOfBirth.HasValue ? CalculateAge(u.DateOfBirth.Value) : null,
                AccountStatus = u.AccountStatus,
                CreatedAt = u.CreatedAt,
                WorkoutCount = u.WorkoutCount,
                DeletedAt = u.DeletedAt,
                WeightEntryCount = u.WeightEntryCount
            })
            .ToList();

        return new PagedResult<GetPagedUsersForAdminResponse>(users, request.Page, request.PageSize, totalCount, users.Count);
    }

    private static IOrderedQueryable<User> ApplySort(IQueryable<User> query, string? sort)
    {
        return sort switch
        {
            "oldest" => query.OrderBy(u => u.CreatedAt).ThenBy(u => u.Id),
            "name" => query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ThenBy(u => u.Id),
            "email" => query.OrderBy(u => u.Email).ThenBy(u => u.Id),
            _ => query.OrderByDescending(u => u.CreatedAt).ThenBy(u => u.Id)
        };
    }

    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
            age--;
        return age;
    }

    private static string GetFullName(User user)
    {
        if (user.FirstName is null || user.LastName is null)
            return "Not specified";
        
        return $"{user.FirstName} {user.LastName}";
    }
}
