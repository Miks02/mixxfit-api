namespace MixxFit.API.Features.Exercises.GetExercisesPage;

public record MuscleGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}