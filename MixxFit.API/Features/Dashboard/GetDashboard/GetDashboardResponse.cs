namespace MixxFit.API.Features.Dashboard.GetDashboard;

public record GetDashboardResponse
{
    public double DailyCalories { get; init; }
    public double WaterIntake { get; init; }
    public double AverageSleep { get; init; }
    public DateTime? LastWorkoutDate { get; init; }
    public IReadOnlyList<RecentWorkoutDto> RecentWorkouts { get; init; } = new List<RecentWorkoutDto>();
}