using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Features.Workouts.GetPagedWorkouts;

public record GetPagedWorkoutsRequest (
    [FromQuery(Name = "page")] int Page = 1, 
    [FromQuery(Name = "pageSize")] int PageSize = 8, 
    [FromQuery(Name = "sort")] string? Sort = null, 
    [FromQuery(Name = "search")] string? Search = null, 
    [FromQuery(Name= "date")] DateTime? Date = null);