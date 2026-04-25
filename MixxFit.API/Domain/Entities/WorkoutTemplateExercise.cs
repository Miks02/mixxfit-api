namespace MixxFit.API.Domain.Entities;

public class WorkoutTemplateExercise
{
    public int WorkoutTemplateId { get; set; }
    public WorkoutTemplate WorkoutTemplate { get; set; } = null!;
    
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public int Order { get; set; }
    public int SetCount { get; set; } = 1;
}