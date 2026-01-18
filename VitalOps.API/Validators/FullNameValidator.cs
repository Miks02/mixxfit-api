using FluentValidation;
using VitalOps.API.DTO.User;

namespace VitalOps.API.Validators
{
    public class FullNameValidator : AbstractValidator<UpdateFullNameDto>
    {
        public FullNameValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name cannot be empty")
                .MinimumLength(2)
                .WithMessage("At least 2 letters required")
                .MaximumLength(20)
                .WithMessage("First name cannot exceed 20 characters");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name cannot be empty")
                .MinimumLength(2)
                .WithMessage("At least 2 letters required")
                .MaximumLength(20)
                .WithMessage("Last name cannot exceed 20 characters");

        }
    }
}
