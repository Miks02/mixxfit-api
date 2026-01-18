using FluentValidation;
using VitalOps.API.DTO.User;

namespace VitalOps.API.Validators
{
    public class GenderValidator : AbstractValidator<UpdateGenderDto>
    {
        public GenderValidator()
        {
            RuleFor(x => x.Gender)
                .IsInEnum()
                .NotEmpty()
                .WithMessage("Gender is required");
        }
    }
}
