using MixxFit.VSA.Domain.Enums;

namespace MixxFit.VSA.Features.Workouts.GetWorkoutsOverview;

public record WorkoutListItemDto
    (int Id, string Name, DateTime WorkoutDate, int ExerciseCount, int SetCount, bool HasCardio, bool HasWeights, bool HasBodyWeight);