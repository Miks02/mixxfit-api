using WorkoutTrackerApi.Enums;

namespace WorkoutTrackerApi.Models;

public class ExerciseEntry
{
    public int Id { get; set; }

    public string ExerciseName { get; set; } = null!;
    public ExerciseType ExerciseType { get; set; }
    
    public int WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;
    
    public double? DistanceKm { get; set; }
    public TimeSpan? Duration { get; set; }
    public int? AvgHeartRate { get; set; }
    public double? CaloriesBurned { get; set; }
    
    public ICollection<SetEntry> Sets { get; set; } = [];

}