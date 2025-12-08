using WorkoutTrackerApi.DTO.SetEntry;
using WorkoutTrackerApi.Enums;

namespace WorkoutTrackerApi.DTO.ExerciseEntry;

public class ExerciseEntryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public ExerciseType ExerciseType { get; set; }
    
    public double? DistanceKm { get; set; }
    public TimeSpan? Duration { get; set; }
    public int? AvgHeartRate { get; set; }
    public double? CaloriesBurned { get; set; }
    
    public IEnumerable<SetEntryDto> Sets { get; set; } = [];
}