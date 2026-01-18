using VitalOps.API.DTO.Global;

namespace VitalOps.API.DTO.Workout
{
    public class WorkoutPageDto
    {
        public PagedResult<WorkoutListItemDto> PagedWorkouts { get; set; } = null!;

        public WorkoutSummaryDto WorkoutSummary { get; set; } = null!;
    }
}
