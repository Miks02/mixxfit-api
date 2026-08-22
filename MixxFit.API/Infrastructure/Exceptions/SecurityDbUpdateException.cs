namespace MixxFit.API.Infrastructure.Exceptions;

public class SecurityDbUpdateException(string message, Exception inner) : Exception(message, inner);