using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.Admins;

public class AdminErrors
{
    public static Error NotFound(string identifier) 
        => new("Admin.NotFound", $"Admin with identifier '{identifier}' was not found", ErrorType.NotFound);
}