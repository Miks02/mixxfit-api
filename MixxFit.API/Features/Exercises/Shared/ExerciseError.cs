using MixxFit.API.Common.Results;

namespace MixxFit.API.Features.Exercises.Shared;

public class ExerciseError
{
    public static Error MuscleGroupNotFound(int id) 
        => new("MuscleGroupNotFound", $"MuscleGroup with id: '{id}' not found");
    
    public static Error ExerciseCategoryNotFound(int id) 
        => new("ExerciseCategoryNotFound", $"ExerciseCategory with id: '{id}' not found");
}