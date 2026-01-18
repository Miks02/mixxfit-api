using FluentValidation;
using VitalOps.API.DTO.User;

namespace VitalOps.API.Validators
{
    public class HeightValidator : AbstractValidator<UpdateHeightDto>
    {
        public HeightValidator()
        {
            RuleFor(x => x.Height)
               .GreaterThanOrEqualTo(70)
               .WithMessage("Height below 70 cm is not supported")
               .LessThanOrEqualTo(250)
               .WithMessage("Height above 250 cm is not supported");

        }
    }
}
