using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Users.GetPagedUsersForAdmin;

public record GetPagedUsersForAdminResponse
{
    public string Id { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public int? Age { get; init; }
    public AccountStatus AccountStatus { get; init; }
    public DateTime CreatedAt { get; init; }
    public int WorkoutCount { get; init; }
    public int WeightEntryCount { get; init; }
}
