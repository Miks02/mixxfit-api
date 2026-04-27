using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.SetEntries;

public class SetEntryError
{
    public static Error NotFound(string message = "Set entry was not found")
        => new("SetEntry.NotFound", message);
}
