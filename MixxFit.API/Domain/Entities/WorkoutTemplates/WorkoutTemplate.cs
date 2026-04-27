using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.WorkoutTemplateExercises;

namespace MixxFit.API.Domain.Entities.WorkoutTemplates;

public class WorkoutTemplate
{
    public int Id { get; set; }

    public int? FitnessProfileId { get; set; }
    public FitnessProfile? FitnessProfile { get; set; }
    
    public string Name { get; set; } = null!;
    public string? Notes { get; set; }
    public ICollection<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; } = [];
}