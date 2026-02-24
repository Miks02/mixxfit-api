namespace MixxFit.VSA.Features.Workouts.GetWorkoutById;

public record SetEntryDto
{
    public int Reps { get; set; }
    public double WeightKg { get; set; }
}