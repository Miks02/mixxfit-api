using MixxFit.API.Domain.Entities.ExerciseCategories;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.MuscleGroups;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.UnitTests.Features.Exercises;

internal static class ExerciseTestData
{
    public const string UserId = "user-1";
    public const string OtherUserId = "user-2";

    public const int CardioCategoryId = 1;
    public const int BarbellCategoryId = 2;
    public const int DumbbellCategoryId = 3;
    public const int OtherCategoryId = 5;
    public const int BodyweightCategoryId = 6;
    public const int DurationCategoryId = 7;
    public const int AssistedBodyweightCategoryId = 8;
    public const int StretchingCategoryId = 9;

    public const int ChestId = 1;
    public const int BackId = 2;
    public const int LegsId = 5;

    public static readonly ExerciseCategory[] Categories =
    [
        new() { Id = CardioCategoryId, Name = "Cardio" },
        new() { Id = BarbellCategoryId, Name = "Barbell" },
        new() { Id = DumbbellCategoryId, Name = "Dumbbell" },
        new() { Id = OtherCategoryId, Name = "Other" },
        new() { Id = BodyweightCategoryId, Name = "Bodyweight" },
        new() { Id = DurationCategoryId, Name = "Duration" },
        new() { Id = AssistedBodyweightCategoryId, Name = "Assisted Bodyweight" },
        new() { Id = StretchingCategoryId, Name = "Stretching" }
    ];

    public static readonly MuscleGroup[] MuscleGroups =
    [
        new() { Id = ChestId, Name = "Chest" },
        new() { Id = BackId, Name = "Back" },
        new() { Id = LegsId, Name = "Legs" }
    ];

    public static void SeedLookups(AppDbContext context, params string[] profileUserIds)
    {
        context.ExerciseCategories.AddRange(Categories.Select(c => new ExerciseCategory { Id = c.Id, Name = c.Name }));
        context.MuscleGroups.AddRange(MuscleGroups.Select(m => new MuscleGroup { Id = m.Id, Name = m.Name }));
        context.FitnessProfiles.AddRange(profileUserIds.Select(id => new FitnessProfile { UserId = id }));
        context.SaveChanges();
    }

    public static Exercise Exercise(
        int id,
        string name,
        string? ownerId = UserId,
        int categoryId = BarbellCategoryId,
        int muscleGroupId = ChestId,
        ExerciseType type = ExerciseType.WeightLifting,
        bool isDeleted = false) => new()
    {
        Id = id,
        Name = name,
        OwnerId = ownerId,
        ExerciseCategoryId = categoryId,
        MuscleGroupId = muscleGroupId,
        ExerciseType = type,
        IsDeleted = isDeleted
    };
}
