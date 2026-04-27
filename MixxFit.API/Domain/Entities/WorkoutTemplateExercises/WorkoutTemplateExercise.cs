using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.WorkoutTemplates;

namespace MixxFit.API.Domain.Entities.WorkoutTemplateExercises;

public class WorkoutTemplateExercise
{
    public int WorkoutTemplateId { get; set; }
    public WorkoutTemplate WorkoutTemplate { get; set; } = null!;
    
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public int Order { get; set; }
    public int SetCount { get; set; } = 1;
}