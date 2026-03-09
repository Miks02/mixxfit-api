using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.GetWorkoutsOverview;

public record WorkoutSummaryDto(int WorkoutCount, int ExerciseCount, DateTime? LastWorkoutDate, ExerciseType FavoriteExerciseType);