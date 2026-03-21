using MixxFit.API.Features.Exercises.Shared;

namespace MixxFit.API.Features.Exercises.GetExercises;

public record GetExercisesResponse
{
    public IReadOnlyList<ExerciseDto> Exercises { get; set; } = [];
}