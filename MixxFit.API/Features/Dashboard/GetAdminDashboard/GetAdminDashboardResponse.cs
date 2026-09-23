using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Dashboard.GetAdminDashboard;

public record GetAdminDashboardResponse
{
    public PagedResult<AdminDashboardUserDto> Users { get; init; } = null!;
    public int TotalExercises { get; init; }
    public int TotalWorkouts { get; init; }
    public ExerciseType? MostCommonExerciseType { get; init; }
    public int TotalWeightEntries { get; init; }
    public double? AverageUserAge { get; init; }
}
