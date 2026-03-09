
using MixxFit.API.Common.Results;

namespace MixxFit.API.Features.Workouts.GetWorkoutsOverview;

public record GetWorkoutsOverviewResponse(PagedResult<WorkoutListItemDto> PagedWorkouts, WorkoutSummaryDto WorkoutSummary);
