namespace MixxFit.API.Features.Workouts.GetWorkouts;

public record GetWorkoutsResponse
{
   public int? Year { get; init; }
   public int? Month { get; init; }
   public IReadOnlyList<int> AvailableYears { get; init; } = [];
   public IReadOnlyList<int> AvailableMonths { get; init; } = [];
   public IReadOnlyList<WorkoutListItemDto> Workouts { get; init; } = [];
}
