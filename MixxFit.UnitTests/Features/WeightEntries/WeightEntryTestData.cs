using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.WeightEntries;

namespace MixxFit.UnitTests.Features.WeightEntries;

internal static class WeightEntryTestData
{
    public const string UserId = "user-1";
    public const string OtherUserId = "user-2";

    public static DateTime Utc(int year, int month, int day, int hour = 0) => new(year, month, day, hour, 0, 0, DateTimeKind.Utc);

    public static WeightEntry Entry(decimal weight, DateTime createdAt, string ownerId = UserId, string? notes = null) => new()
    {
        Weight = weight,
        OwnerId = ownerId,
        CreatedAt = createdAt,
        Time = createdAt.TimeOfDay,
        Notes = notes
    };

    public static User UserWithProfile(string userId, decimal? weight = null) => new()
    {
        Id = userId,
        UserName = userId,
        Email = $"{userId}@mail.com",
        FitnessProfile = new FitnessProfile { UserId = userId, Weight = weight }
    };
}
