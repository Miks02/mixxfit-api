using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Exercises.CreateExercise;

public record CreateExerciseRequest
{
    public string Name { get; init; } = null!;
    public int CategoryId { get; init; }
    public int MuscleGroupId { get; init; }
};