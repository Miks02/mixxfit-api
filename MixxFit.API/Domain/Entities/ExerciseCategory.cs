namespace MixxFit.API.Domain.Entities;

public class ExerciseCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public ICollection<Exercise> Exercises { get; set; } = [];
}