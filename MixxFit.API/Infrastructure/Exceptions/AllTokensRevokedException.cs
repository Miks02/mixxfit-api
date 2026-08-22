namespace MixxFit.API.Infrastructure.Exceptions;

public class AllTokensRevokedException(
    string message = "Security breach has been detected. All tokens have been revoked.")
    : Exception(message);