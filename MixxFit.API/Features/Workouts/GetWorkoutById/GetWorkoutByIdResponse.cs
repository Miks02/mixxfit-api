
namespace MixxFit.API.Features.Workouts.GetWorkoutById;

public record GetWorkoutByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string? Notes { get; set; }
    
    public int FitnessProfileId { get; set; }

    public DateTime WorkoutDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public IEnumerable<ExerciseEntryDto> Exercises { get; set; } = [];
}