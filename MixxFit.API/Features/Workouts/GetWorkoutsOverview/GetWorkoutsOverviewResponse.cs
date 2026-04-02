namespace MixxFit.API.Features.Workouts.GetWorkoutsOverview;

public record GetWorkoutsOverviewResponse(
    int? Year,
    int? Month,
    IReadOnlyList<int> AvailableYears,
    IReadOnlyList<int> AvailableMonths,
    IReadOnlyList<WorkoutListItemDto> Workouts,
    WorkoutSummaryDto WorkoutSummary);
