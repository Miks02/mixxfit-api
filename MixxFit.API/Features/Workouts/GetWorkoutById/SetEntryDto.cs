namespace MixxFit.API.Features.Workouts.GetWorkoutById;

public record SetEntryDto
{
    public int? Reps { get; set; }
    public decimal? Weight { get; set; }
    
    public decimal? Distance { get; set; }
    public int? DurationMinutes { get; set; }
    public int? DurationSeconds { get; set; }
}