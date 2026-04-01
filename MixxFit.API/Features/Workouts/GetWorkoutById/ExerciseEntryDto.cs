using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Workouts.GetWorkoutById;

public record ExerciseEntryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ExerciseType ExerciseType { get; set; }
    public IEnumerable<SetEntryDto> Sets { get; set; } = [];
}