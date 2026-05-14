using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Domain.Entities.FitnessProfiles;

public class FitnessProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    
    public Gender? Gender { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public double? DailyCalorieGoal { get; set; }
    public double? TargetWeight { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ICollection<WorkoutTemplate> WorkoutTemplates { get; set; } = [];
    public ICollection<WeightEntry> WeightEntries { get; set; } = [];
    public ICollection<Workout> Workouts { get; set; } = [];
    public ICollection<Exercise> Exercises { get; set; } = [];
}