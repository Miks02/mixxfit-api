using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.Exercises;

public class ExerciseError
{
    public static Error MuscleGroupNotFound(int id) 
        => new("Exercise.MuscleGroupNotFound", $"Muscle group with id '{id}' was not found", ErrorType.NotFound);
    
    public static Error ExerciseCategoryNotFound(int id) 
        => new("Exercise.ExerciseCategoryNotFound", $"Exercise category with id '{id}' was not found", ErrorType.NotFound);

    public static Error NotFound(string message = "One or more selected exercises are no longer available")
        => new("Exercise.NotFound", message, ErrorType.NotFound);

    public static Error AlreadyExists(string message = "Exercise already exists")
        => new("Exercise.AlreadyExists", message, ErrorType.Conflict);  

}