using MixxFit.API.Domain.Entities.Exercises;

namespace MixxFit.API.Domain.Entities.MuscleGroups;

public class MuscleGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Exercise> Exercises { get; set; } = [];
}