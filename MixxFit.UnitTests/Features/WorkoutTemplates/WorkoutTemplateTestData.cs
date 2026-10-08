using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.WorkoutTemplateExercises;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Domain.Enums;

namespace MixxFit.UnitTests.Features.WorkoutTemplates;

internal static class WorkoutTemplateTestData
{
    public const string UserId = "user-1";
    public const string OtherUserId = "user-2";

    public static WorkoutTemplate Template(
        string name,
        string? ownerId = UserId,
        string? notes = null,
        params (int ExerciseId, int SetCount, int Order)[] exercises) => new()
    {
        Name = name,
        OwnerId = ownerId,
        Notes = notes,
        WorkoutTemplateExercises = exercises.Select(e => new WorkoutTemplateExercise
        {
            ExerciseId = e.ExerciseId,
            SetCount = e.SetCount,
            Order = e.Order
        }).ToList()
    };

    public static Exercise Exercise(int id, string? ownerId = null, bool isDeleted = false) => new()
    {
        Id = id,
        Name = $"Exercise {id}",
        OwnerId = ownerId,
        ExerciseType = ExerciseType.WeightLifting,
        ExerciseCategoryId = 1,
        MuscleGroupId = 1,
        IsDeleted = isDeleted
    };

    public static User UserWithProfile(string userId) => new()
    {
        Id = userId,
        UserName = userId,
        Email = $"{userId}@mail.com",
        FitnessProfile = new FitnessProfile { UserId = userId }
    };
}
