using FluentValidation;

namespace MixxFit.VSA.Features.Auth.Login;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty()
            .WithMessage("Email is required");

        RuleFor(l => l.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}