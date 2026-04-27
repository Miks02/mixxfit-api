using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.MuscleGroups;

public class MuscleGroupError
{
    public static Error NotFound(string message = "Muscle group was not found")
        => new("MuscleGroup.NotFound", message);

    public static Error AlreadyExists(string message = "Muscle group already exists")
        => new("MuscleGroup.AlreadyExists", message);
}
