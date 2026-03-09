using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Features.Workouts.GetWorkoutChartData;

public class GetWorkoutChartDataRequest
{
    [FromQuery(Name = "year")]
    public int? Year { get; set; }
}