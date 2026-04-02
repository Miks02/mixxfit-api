using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.GetWorkoutsOverview;

public record GetWorkoutsOverviewRequest (
    [FromQuery(Name = "sort")] string? Sort = null, 
    [FromQuery(Name = "search")] string? Search = null, 
    [FromQuery(Name = "year")] int? Year = null,
    [FromQuery(Name = "month")] Month? Month = null);
