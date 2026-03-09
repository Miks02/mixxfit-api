
namespace MixxFit.API.Features.Workouts.GetWorkoutById;

public record GetWorkoutByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string? Notes { get; set; }
    
    public string UserId { get; set; } = null!;

    public DateTime WorkoutDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public IEnumerable<ExerciseEntryDto> Exercises { get; set; } = [];
}