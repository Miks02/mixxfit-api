namespace MixxFit.VSA.Features.Workouts.CreateWorkout;

public record CreateWorkoutRequest
{
    public string Name { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTime WorkoutDate { get; set; }
    public List<ExerciseEntryDto> ExerciseEntries { get; set; } = [];
}