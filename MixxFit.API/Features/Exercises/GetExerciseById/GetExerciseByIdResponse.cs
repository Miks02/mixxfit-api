namespace MixxFit.API.Features.Exercises.GetExerciseById;

public record GetExerciseByIdResponse
{
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public int MuscleGroupId { get; set; }
}