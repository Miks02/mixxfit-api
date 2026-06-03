using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.GetWorkoutsSummary;

public record MostActiveMonthDto
{
    public Month Month { get; set; }
    public int Year { get; set; }
    public int WorkoutCount { get; set; }
};