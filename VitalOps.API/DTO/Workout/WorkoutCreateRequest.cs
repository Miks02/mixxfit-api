using VitalOps.API.DTO.ExerciseEntry;

namespace VitalOps.API.DTO.Workout;

public class WorkoutCreateRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTime WorkoutDate { get; set; }
    
    public string UserId { get; set; } = null!;

    public List<ExerciseEntryDto> ExerciseEntries { get; set; } = [];
}