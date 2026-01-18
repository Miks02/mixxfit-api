using FluentValidation;
using VitalOps.API.DTO.User;

namespace VitalOps.API.Validators
{
    public class WeightValidator : AbstractValidator<UpdateWeightDto>
    {
        public WeightValidator()
        {
            RuleFor(x => x.Weight)
                .GreaterThan(25)
                .WithMessage("Weight must be greater than 25 KG")
                .LessThanOrEqualTo(400)
                .WithMessage("Weight cannot be higher than 400 KG");
        }
    }
}
