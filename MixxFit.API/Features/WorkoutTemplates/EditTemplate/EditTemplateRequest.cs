namespace MixxFit.API.Features.WorkoutTemplates.EditTemplate;

public record EditTemplateRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Notes { get; set; } 

    public IReadOnlyList<ExerciseItem> Exercises { get; set; } = null!;
    
    public record ExerciseItem
    {
        public int ExerciseId { get; set; }
        public int SetCount { get; set; }
    }
}