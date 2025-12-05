namespace WorkoutTrackerApi.Models;

public class CalorieEntry
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = null!;
    public int Calories { get; set; }

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}