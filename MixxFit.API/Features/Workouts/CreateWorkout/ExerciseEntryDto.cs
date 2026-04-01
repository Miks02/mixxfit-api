using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.CreateWorkout;

public record ExerciseEntryDto
{
    public int ExerciseId { get; set; }
    public string Name { get; set; } = null!;
    public ExerciseType ExerciseType { get; set; }
    public IEnumerable<SetEntryDto> Sets { get; set; } = [];
}