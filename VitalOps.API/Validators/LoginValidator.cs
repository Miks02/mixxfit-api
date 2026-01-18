using FluentValidation;
using VitalOps.API.DTO.Auth;

namespace VitalOps.API.Validators;

public class LoginValidator : AbstractValidator<LoginRequestDto>
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