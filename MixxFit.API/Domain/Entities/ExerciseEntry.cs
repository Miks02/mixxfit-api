using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Domain.Entities;

public class ExerciseEntry
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public int? ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public ExerciseType ExerciseType { get; set; }
    
    public int WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;
    
    public ICollection<SetEntry> Sets { get; set; } = [];

}