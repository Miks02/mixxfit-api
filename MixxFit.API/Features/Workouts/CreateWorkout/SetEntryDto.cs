namespace MixxFit.API.Features.Workouts.CreateWorkout;

public record SetEntryDto
{
    public int Reps { get; set; }
    public double WeightKg { get; set; }
}