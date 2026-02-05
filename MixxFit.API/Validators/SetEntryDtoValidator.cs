using FluentValidation;
using MixxFit.API.DTO.SetEntry;

namespace MixxFit.API.Validators;

public class SetEntryDtoValidator : AbstractValidator<SetEntryDto>
{
    public SetEntryDtoValidator()
    {
        RuleFor(p => p.Reps)
            .GreaterThan(0)
            .WithMessage("At least 1 repetition is required")
            .LessThan(1001)
            .WithMessage("Number of reps can be between 1 and 1000");

        RuleFor(p => p.WeightKg)
            .GreaterThan(0)
            .WithMessage("Weight is required")
            .LessThan(1001)
            .WithMessage("Amount of weight can be between 1 and 1000 kg's");
    }
}