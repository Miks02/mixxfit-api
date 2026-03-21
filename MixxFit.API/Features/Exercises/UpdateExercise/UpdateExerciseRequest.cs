namespace MixxFit.API.Features.Exercises.UpdateExercise;

public record UpdateExerciseRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public int MuscleGroupId { get; set; }

};