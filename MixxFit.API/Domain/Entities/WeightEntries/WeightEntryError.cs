using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.WeightEntries;

public class WeightEntryError
{
    public static Error NotFound(string message = "Weight entry was not found")
        => new("WeightEntry.NotFound", message, ErrorType.NotFound);

    public static Error LimitReached(string message = "Weight entry limit has been reached")
        => new("WeightEntry.LimitReached", message, ErrorType.TooManyRequests);
}
