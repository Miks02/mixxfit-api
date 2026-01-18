using VitalOps.API.DTO.Workout;

namespace VitalOps.API.DTO.User
{
    public class ProfilePageDto
    {
        public IReadOnlyList<WorkoutListItemDto> RecentWorkouts { get; set; } = [];
        public int? WorkoutStreak { get; set; }
        public int? DailyCalorieGoal { get; set; }

    }
}
