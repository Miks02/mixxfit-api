namespace MixxFit.API.Features.WorkoutTemplates.CreateTemplate;

public record TemplateExerciseDto
{
    public int ExerciseId { get; set; }
    public int SetCount { get; set; }
    public int Order { get; set; }
};