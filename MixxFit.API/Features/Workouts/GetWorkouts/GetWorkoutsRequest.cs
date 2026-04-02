using Microsoft.AspNetCore.Mvc;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.GetWorkouts;

public record GetWorkoutsRequest
{
    [FromQuery]
    public string? Sort { get; init; } 
    [FromQuery]
    public string? Search { get; init;  } 
    
    [FromQuery]
    public int? Year { get; init; }
    [FromQuery]
    public Month? Month { get; init; } 
}
