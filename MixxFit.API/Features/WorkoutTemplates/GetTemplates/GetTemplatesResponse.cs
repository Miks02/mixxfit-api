namespace MixxFit.API.Features.WorkoutTemplates.GetTemplates;

public record GetTemplatesResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Notes { get; set; }
    public bool IsSystem { get; set; }
    public IReadOnlyList<TemplateExerciseDto> Exercises { get; set; } = null!;
};