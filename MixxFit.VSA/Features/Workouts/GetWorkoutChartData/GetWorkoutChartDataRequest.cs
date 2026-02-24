using Microsoft.AspNetCore.Mvc;

namespace MixxFit.VSA.Features.Workouts.GetWorkoutChartData;

public class GetWorkoutChartDataRequest
{
    [FromQuery(Name = "year")]
    public int? Year { get; set; }
}