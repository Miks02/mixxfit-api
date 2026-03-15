using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Exercises.GetExercisesPage;

public record ExerciseDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string Name { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public string MuscleGroupName { get; set; } = null!;
    public ExerciseType ExerciseType { get; set; }
}