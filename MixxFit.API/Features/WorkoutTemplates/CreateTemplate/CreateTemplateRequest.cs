namespace MixxFit.API.Features.WorkoutTemplates.CreateTemplate;

public record CreateTemplateRequest
{
    public string Name { get; set; } = null!;
    public string? Notes { get; set; } 

    public IReadOnlyList<ExerciseItem> Exercises { get; set; } = null!;
    
    public record ExerciseItem
    {
        public int ExerciseId { get; set; }
        public int SetCount { get; set; }
    }
};