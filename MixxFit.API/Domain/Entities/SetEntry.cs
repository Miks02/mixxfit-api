namespace MixxFit.API.Domain.Entities;

public class SetEntry
{
    public int Id { get; set; }
    
    public int ExerciseEntryId { get; set; }
    public ExerciseEntry ExerciseEntry { get; set; } = null!;
    
    public int? Reps { get; set; }
    public decimal? Weight { get; set; }
    
    public decimal? Distance { get; set; }
    public int? DurationSeconds { get; set; }
}