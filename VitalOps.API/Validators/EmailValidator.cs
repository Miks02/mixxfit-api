using FluentValidation;
using VitalOps.API.DTO.User;

namespace VitalOps.API.Validators
{
    public class EmailValidator : AbstractValidator<UpdateEmailDto>
    {
        public EmailValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email address");
                
        }
    }
}
