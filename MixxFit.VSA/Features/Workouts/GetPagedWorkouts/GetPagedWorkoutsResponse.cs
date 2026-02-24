namespace MixxFit.VSA.Features.Workouts.GetPagedWorkouts;

public record GetPagedWorkoutsResponse(
    int Id, 
    string Name, 
    DateTime WorkoutDate, 
    int ExerciseCount, 
    int SetCount, 
    bool HasCardio,
    bool HasWeights,
    bool HasBodyWeight);