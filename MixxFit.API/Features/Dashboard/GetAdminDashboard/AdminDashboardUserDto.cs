using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Dashboard.GetAdminDashboard;

public record AdminDashboardUserDto
{
    public string Id { get; init; } = null!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string Email { get; init; } = null!;
    public int? Age { get; init; }
    public AccountStatus AccountStatus { get; init; }
    public DateTime CreatedAt { get; init; }
    public int WorkoutCount { get; init; }
    public int WeightEntryCount { get; init; }
}
