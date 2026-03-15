namespace MixxFit.API.Features.Exercises.GetExercisesPage;

public record ExerciseCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}