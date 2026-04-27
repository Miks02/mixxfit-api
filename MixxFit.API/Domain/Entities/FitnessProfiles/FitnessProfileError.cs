using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.FitnessProfiles;

namespace MixxFit.API.Domain.Entities.FitnessProfiles;

public class FitnessProfileError
{
    public static Error NotFound(string message = "Fitness profile was not found")
        => new("FitnessProfile.NotFound", message);
}
