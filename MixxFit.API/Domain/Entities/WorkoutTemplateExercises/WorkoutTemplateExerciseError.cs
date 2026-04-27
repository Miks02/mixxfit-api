using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.WorkoutTemplateExercises;

public class WorkoutTemplateExerciseError
{
    public static Error NotFound(string message = "Workout template exercise was not found")
        => new("WorkoutTemplateExercise.NotFound", message);

    public static Error AlreadyExists(string message = "Workout template exercise already exists")
        => new("WorkoutTemplateExercise.AlreadyExists", message);
}
