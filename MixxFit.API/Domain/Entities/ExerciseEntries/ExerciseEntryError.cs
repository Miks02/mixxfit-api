using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.ExerciseEntries;

public class ExerciseEntryError
{
    public static Error NotFound(string message = "Exercise entry was not found")
        => new("ExerciseEntry.NotFound", message);
}
