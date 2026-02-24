
using MixxFit.VSA.Common.Results;

namespace MixxFit.VSA.Features.Workouts.GetWorkoutsOverview;

public record GetWorkoutsOverviewResponse(PagedResult<WorkoutListItemDto> PagedWorkouts, WorkoutSummaryDto WorkoutSummary);
