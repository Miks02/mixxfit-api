using MixxFit.API.Common.Results;

namespace MixxFit.API.Features.Exercises.Shared;

public class ExerciseError
{
    public static Error MuscleGroupNotFound(int id) 
        => new("Exercise.MuscleGroupNotFound", $"MuscleGroup with id: '{id}' not found");
    
    public static Error ExerciseCategoryNotFound(int id) 
        => new("Exercise.ExerciseCategoryNotFound", $"ExerciseCategory with id: '{id}' not found");

    public static Error NotFound(string message = "One or more selected exercises are no longer available")
        => new("Exercise.NotFound", message);

    public static Error AlreadyExists(string message = "Exercise already exists")
        => new("Exercise.AlreadyExists", message);

}