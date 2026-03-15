using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Domain.Entities;

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ExerciseType ExerciseType { get; set; }
    
    public string? UserId { get; set; } 
    public User? User { get; set; }
    
    public ICollection<ExerciseEntry> ExerciseEntries { get; set; } = [];
    
    public int ExerciseCategoryId { get; set; }
    public ExerciseCategory ExerciseCategory { get; set; } = null!;
    
    public int MuscleGroupId { get; set; }
    public MuscleGroup MuscleGroup { get; set; } = null!;
}