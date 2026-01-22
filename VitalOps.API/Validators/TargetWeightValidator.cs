using FluentValidation;
using VitalOps.API.DTO.User;

namespace VitalOps.API.Validators
{
    public class TargetWeightValidator : AbstractValidator<UpdateWeightDto>
    {
        public TargetWeightValidator()
        {
            RuleFor(x => x.Weight)
                .GreaterThan(25)
                .WithMessage("Target weight must be greater than 25 KG")
                .LessThanOrEqualTo(400)
                .WithMessage("Target weight cannot be higher than 400 KG");
        }
    }
}
