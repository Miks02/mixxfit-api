namespace MixxFit.API.Features.WorkoutTemplates.Common;

public record TemplateExerciseDto
{
    public int ExerciseId { get; set; }
    public int SetCount { get; set; }
    public int Order { get; set; }
};