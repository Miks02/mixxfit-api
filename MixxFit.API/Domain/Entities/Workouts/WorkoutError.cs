using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.Workouts;

public class WorkoutError
{
    public static Error NotFound(string message = "Workout was not found")
        => new("Workout.NotFound", message);

    public static Error LimitReached(string message = "Workout limit has been reached")
        => new("Workout.LimitReached", message);
}
