using FluentValidation;
using VitalOps.API.DTO.SetEntry;

namespace VitalOps.API.Validators;

public class SetEntryDtoValidator : AbstractValidator<SetEntryDto>
{
    public SetEntryDtoValidator()
    {
        RuleFor(p => p.Reps)
            .GreaterThan(0)
            .WithMessage("At least 1 repetition is required");

        RuleFor(p => p.WeightKg)
            .GreaterThan(0)
            .WithMessage("Weight is required");
    }
}