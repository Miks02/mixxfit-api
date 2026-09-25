using MixxFit.API.Domain.Entities.ExerciseEntries;
using MixxFit.API.Domain.Entities.SetEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Enums;

namespace MixxFit.UnitTests.Features.Workouts;

internal static class WorkoutTestData
{
    public const string UserId = "user-1";
    public const string OtherUserId = "user-2";

    public static DateTime Utc(int year, int month, int day) => new(year, month, day, 0, 0, 0, DateTimeKind.Utc);

    public static Workout Workout(
        DateTime workoutDate,
        string name = "Workout",
        string ownerId = UserId,
        params ExerciseEntry[] entries) => new()
    {
        Name = name,
        OwnerId = ownerId,
        WorkoutDate = workoutDate,
        CreatedAt = workoutDate,
        ExerciseEntries = entries.ToList()
    };

    public static ExerciseEntry Entry(ExerciseType type, int sets = 1, int exerciseId = 1, string name = "Exercise") => new()
    {
        ExerciseId = exerciseId,
        Name = name,
        ExerciseType = type,
        Sets = Enumerable.Range(0, sets).Select(_ => new SetEntry { Reps = 10, Weight = 50m }).ToList()
    };
}
