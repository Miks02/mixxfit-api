namespace MixxFit.API.Features.Dashboard.GetDashboard;

public record RecentWorkoutDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;
    
    public DateTime WorkoutDate { get; init; }
    
    public int ExerciseCount { get; init; }
     
    public int SetCount { get; init; }

    public bool HasCardio { get; init; }
    public bool HasWeights { get; init; }
    public bool HasBodyWeight { get; init; }
};