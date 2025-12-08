namespace WorkoutTrackerApi.DTO.Workout;

public class WorkoutQueryParams
{
    public int Page { get; set; }
    public int PageSize { get; set; } 
    public string? Search { get; set; }
    public string Sort { get; set; }
    public string? UserId { get; set; }

    public WorkoutQueryParams(int page, int pageSize, string? search, string sort, string userId = "")
    {
        Page = page;
        PageSize = pageSize;
        Search = search;
        Sort = sort;
        UserId = userId;
    }
}