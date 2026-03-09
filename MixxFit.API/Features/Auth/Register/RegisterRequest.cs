namespace MixxFit.API.Features.Auth.Register;

public record RegisterRequest(string FirstName, string LastName, string Email, string UserName, string Password, string ConfirmPassword );