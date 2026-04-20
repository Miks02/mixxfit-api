namespace MixxFit.API.Features.WorkoutTemplates.GetTemplates;

public record TemplateExerciseDto
{
    public int ExerciseId { get; set; }
    public int Order { get; set; }
    public int SetCount { get; set; }
};