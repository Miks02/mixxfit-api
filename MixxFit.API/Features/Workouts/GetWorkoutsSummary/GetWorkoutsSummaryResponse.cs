using System.Text.Json.Serialization;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Workouts.GetWorkoutsSummary;

public record GetWorkoutsSummaryResponse
{
    public int WorkoutCount { get; set; }
    public int ExerciseCount { get; set; }
    public DateOnly LastWorkoutDate { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExerciseType FavoriteExerciseType { get; set; }
    public int WorkoutStreak { get; set; }
    public IReadOnlyList<MostActiveMonthDto> MostActiveMonths { get; set; } = [];
}