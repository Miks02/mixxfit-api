using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.GetWorkoutsOverview;

public record WorkoutListItemDto
    (int Id, string Name, DateTime WorkoutDate, int ExerciseCount, int SetCount, bool HasCardio, bool HasWeights, bool HasBodyWeight);