using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Exercises.GetExercises;

public record ExerciseDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string Name { get; set; } = null!;
    public ExerciseType ExerciseType { get; set; }
    public string MuscleGroupName { get; set; } = null!;
}