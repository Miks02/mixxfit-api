using MixxFit.API.Features.WorkoutTemplates.Common;

namespace MixxFit.API.Features.WorkoutTemplates.EditTemplate;

public record EditTemplateResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Notes { get; set; } 
    public IReadOnlyList<TemplateExerciseDto> Exercises { get; set; } = null!;
}