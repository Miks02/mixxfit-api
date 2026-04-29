using MixxFit.API.Features.WorkoutTemplates.Common;

namespace MixxFit.API.Features.WorkoutTemplates.GetTemplateById;

public record GetTemplateByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Notes { get; set; } 
    public bool IsSystem { get; set; }
    public IReadOnlyList<TemplateExerciseDto> Exercises { get; set; } = null!;
};