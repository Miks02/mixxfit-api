using FluentValidation;
using VitalOps.API.DTO.User;

namespace VitalOps.API.Validators
{
    public class UsernameValidator : AbstractValidator<UpdateUserNameDto>
    {
        public UsernameValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MinimumLength(4)
                .WithMessage("At least 2 characters are required")
                .MaximumLength(20)
                .WithMessage("Username cannot exceed 20 characters");
        }
    }
}
