using MixxFit.VSA.Domain.Enums;

namespace MixxFit.VSA.Features.Workouts.GetWorkoutsOverview;

public record WorkoutSummaryDto(int WorkoutCount, int ExerciseCount, DateTime? LastWorkoutDate, ExerciseType FavoriteExerciseType);