namespace MixxFit.API.Features.Exercises.GetExercisesPage;

public record GetExercisesPageResponse
{
    public IReadOnlyList<ExerciseCategoryDto> ExerciseCategories { get; set; } = [];
    public IReadOnlyList<MuscleGroupDto> MuscleGroups { get; set; } = [];
    public IReadOnlyList<ExerciseDto> Exercises { get; set; } = [];
}