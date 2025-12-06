namespace WorkoutTrackerApi.DTO.Workout;

public class WorkoutQueryParams
{
    public string? UserId { get; set; }
    public bool IsAdmin { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? Sort { get; set; } = "newest";
}