namespace MixxFit.API.Features.WorkoutTemplates.CreateTemplate;

public record CreateTemplateResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Notes { get; set; } 
    public IReadOnlyList<TemplateExerciseDto> Exercises { get; set; } = null!;
};