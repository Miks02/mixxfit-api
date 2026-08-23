using MixxFit.API.Common.Results;

namespace MixxFit.API.Domain.Entities.ExerciseCategories;

public class ExerciseCategoryError
{
    public static Error NotFound(string message = "Exercise category was not found")
        => new("ExerciseCategory.NotFound", message, ErrorType.NotFound);

    public static Error AlreadyExists(string message = "Exercise category already exists")
        => new("ExerciseCategory.AlreadyExists", message, ErrorType.Conflict);
}

