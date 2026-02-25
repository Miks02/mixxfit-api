using FluentValidation;

namespace MixxFit.VSA.Features.WeightEntries.LogWeight;

public class LogWeightValidator : AbstractValidator<LogWeightRequest>
{
    public LogWeightValidator()
    {
        RuleFor(x => x.Weight)
            .NotEmpty()
            .WithMessage("Weight is required")
            .GreaterThan(25)
            .WithMessage("Weight has to be higher than 25 KG")
            .LessThan(400)
            .WithMessage("Weight has to be lower than 400 KG");

        RuleFor(x => x.Notes)
            .MaximumLength(100)
            .WithMessage("Maximum length is 100 characters");
    }
}