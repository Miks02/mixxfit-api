using System;
using VitalOps.API.Enums;

namespace VitalOps.API.DTO.Workout;

public class WorkoutSummaryDto
{
    public int WorkoutCount { get; set; }
    public int ExerciseCount { get; set; }
    public DateTime? LastWorkoutDate { get; set; }
    public ExerciseType FavoriteExerciseType { get; set; }
}