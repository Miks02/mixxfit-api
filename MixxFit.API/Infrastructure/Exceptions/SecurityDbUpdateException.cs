namespace MixxFit.API.Infrastructure.Exceptions;

public class SecurityDbUpdateException(string userId, string message, Exception inner) : Exception(message, inner)
{
    public string UserId => userId;
}