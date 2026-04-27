using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.WorkoutTemplates;

namespace MixxFit.API.Domain.Entities.WorkoutTemplates;

public class WorkoutTemplateError
{
    public static Error NotFound(string message = "Workout template was not found")
        => new("WorkoutTemplate.NotFound", message);

    public static Error AlreadyExists(string message = "Workout template already exists")
        => new("WorkoutTemplate.AlreadyExists", message);

    public static Error LimitReached(string message = "Workout template limit has been reached")
        => new("WorkoutTemplate.LimitReached", message);
}
