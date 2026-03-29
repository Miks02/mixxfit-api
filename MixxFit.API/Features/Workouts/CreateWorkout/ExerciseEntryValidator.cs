using FluentValidation;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.CreateWorkout;

public class ExerciseEntryValidator : AbstractValidator<ExerciseEntryDto>
{
    public ExerciseEntryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Exercise name is required")
            .MaximumLength(100)
            .WithMessage("Maximum length is 100 characters");

        RuleFor(x => x.ExerciseType)
            .IsInEnum();
        
        RuleFor(x => x.Sets)
            .NotEmpty()
            .WithMessage("Sets are required");
        
        RuleForEach(x => x.Sets)
            .SetValidator(x => new SetEntryValidator(x.ExerciseType));
    }
    
}