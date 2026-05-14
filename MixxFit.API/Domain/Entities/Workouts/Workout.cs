using MixxFit.API.Domain.Entities.ExerciseEntries;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;

namespace MixxFit.API.Domain.Entities.Workouts;

public class Workout
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string? Notes { get; set; }
    
    public FitnessProfile FitnessProfile { get; set; }
    public int FitnessProfileId { get; set; }

    public DateTime WorkoutDate { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ExerciseEntry> ExerciseEntries { get; set; } = [];

}