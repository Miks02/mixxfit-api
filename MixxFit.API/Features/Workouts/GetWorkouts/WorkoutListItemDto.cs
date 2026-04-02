namespace MixxFit.API.Features.Workouts.GetWorkouts;

public record WorkoutListItemDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public DateTime WorkoutDate { get; init; }
    public int ExerciseCount { get; init; }
    public int SetCount { get; init; }
    public bool HasCardio { get; init; }
    public bool HasWeights { get; init; }
    public bool HasBodyWeight { get; init; }
}
