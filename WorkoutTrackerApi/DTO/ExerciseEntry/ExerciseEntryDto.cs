using WorkoutTrackerApi.DTO.SetEntry;

namespace WorkoutTrackerApi.DTO.ExerciseEntry;

public class ExerciseEntryDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string ExerciseType { get; set; } = null!;
    
    public double? DistanceKm { get; set; }
    public TimeSpan? Duration { get; set; }
    public int? AvgHeartRate { get; set; }
    public double? CaloriesBurned { get; set; }
    
    public List<SetEntryDto> Sets { get; set; } = [];
}