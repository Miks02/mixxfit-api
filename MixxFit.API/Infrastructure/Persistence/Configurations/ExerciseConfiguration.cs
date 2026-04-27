
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50)
            .UseCollation("my_case_insensitive");

        builder
            .HasIndex(p => new { p.Name, p.UserId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = FALSE");

        builder
            .HasIndex(p => new { p.Name, p.ExerciseCategoryId })
            .IsUnique()
            .HasFilter("\"UserId\" IS NULL AND \"IsDeleted\" = FALSE");

        builder
            .HasOne(p => p.User)
            .WithMany(u => u.Exercises)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.ExerciseCategory)
            .WithMany(p => p.Exercises)
            .HasForeignKey(p => p.ExerciseCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.MuscleGroup)
            .WithMany(p => p.Exercises)
            .HasForeignKey(p => p.MuscleGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(p => p.ExerciseEntries)
            .WithOne(ee => ee.Exercise)
            .HasForeignKey(ee => ee.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasQueryFilter(e => e.IsDeleted == false);

        builder.HasData(GetExercises());
    }

    private static IReadOnlyList<Exercise> GetExercises()
    {
        var updatedData = new DateTime(2026, 3, 16, 13, 3, 37, 171, DateTimeKind.Utc);

        var id = 1;
        var exercises = new List<Exercise>();

        Exercise E(string name, ExerciseType type, int categoryId, int muscleGroupId)
            => new()
            {
                Id = id++,
                Name = name,
                ExerciseType = type,
                ExerciseCategoryId = categoryId,
                MuscleGroupId = muscleGroupId,
                CreatedAt = updatedData
            };

        exercises.Add(E("Bench Press", ExerciseType.WeightLifting, 2, 1));
        exercises.Add(E("Incline Bench Press", ExerciseType.WeightLifting, 2, 1));
        exercises.Add(E("Decline Bench Press", ExerciseType.WeightLifting, 2, 1));
        exercises.Add(E("Close Grip Bench Press", ExerciseType.WeightLifting, 2, 1));
        exercises.Add(E("Floor Press", ExerciseType.WeightLifting, 2, 1));
        exercises.Add(E("Guillotine Press", ExerciseType.WeightLifting, 2, 1));

        exercises.Add(E("Bent Over Row", ExerciseType.WeightLifting, 2, 2));
        exercises.Add(E("Pendlay Row", ExerciseType.WeightLifting, 2, 2));
        exercises.Add(E("T-Bar Row", ExerciseType.WeightLifting, 2, 2));
        exercises.Add(E("Deadlift", ExerciseType.WeightLifting, 2, 2));
        exercises.Add(E("Rack Pull", ExerciseType.WeightLifting, 2, 2));
        exercises.Add(E("Seal Row", ExerciseType.WeightLifting, 2, 2));

        exercises.Add(E("Overhead Press", ExerciseType.WeightLifting, 2, 3));
        exercises.Add(E("Behind Neck Press", ExerciseType.WeightLifting, 2, 3));
        exercises.Add(E("Upright Row", ExerciseType.WeightLifting, 2, 3));
        exercises.Add(E("Front Raise", ExerciseType.WeightLifting, 2, 3));
        exercises.Add(E("Bradford Press", ExerciseType.WeightLifting, 2, 3));

        exercises.Add(E("Curl", ExerciseType.WeightLifting, 2, 4));
        exercises.Add(E("Preacher Curl", ExerciseType.WeightLifting, 2, 4));
        exercises.Add(E("Reverse Curl", ExerciseType.WeightLifting, 2, 4));
        exercises.Add(E("Skull Crusher", ExerciseType.WeightLifting, 2, 4));
        exercises.Add(E("Spider Curl", ExerciseType.WeightLifting, 2, 4));
        exercises.Add(E("EZ Bar Curl", ExerciseType.WeightLifting, 2, 4));
        exercises.Add(E("EZ Bar Skull Crusher", ExerciseType.WeightLifting, 2, 4));

        exercises.Add(E("Squat", ExerciseType.WeightLifting, 2, 5));
        exercises.Add(E("Front Squat", ExerciseType.WeightLifting, 2, 5));
        exercises.Add(E("Lunge", ExerciseType.WeightLifting, 2, 5));
        exercises.Add(E("Romanian Deadlift", ExerciseType.WeightLifting, 2, 5));
        exercises.Add(E("Stiff Leg Deadlift", ExerciseType.WeightLifting, 2, 5));
        exercises.Add(E("Calf Raise", ExerciseType.WeightLifting, 2, 5));
        exercises.Add(E("Good Morning", ExerciseType.WeightLifting, 2, 5));
        exercises.Add(E("Zercher Squat", ExerciseType.WeightLifting, 2, 5));
        exercises.Add(E("Hack Squat", ExerciseType.WeightLifting, 2, 5));

        exercises.Add(E("Rollout", ExerciseType.WeightLifting, 2, 6));
        exercises.Add(E("Landmine Rotation", ExerciseType.WeightLifting, 2, 6));

        exercises.Add(E("Hip Thrust", ExerciseType.WeightLifting, 2, 7));
        exercises.Add(E("Glute Bridge", ExerciseType.WeightLifting, 2, 7));
        exercises.Add(E("Sumo Deadlift", ExerciseType.WeightLifting, 2, 7));

        exercises.Add(E("Bench Press", ExerciseType.WeightLifting, 3, 1));
        exercises.Add(E("Incline Bench Press", ExerciseType.WeightLifting, 3, 1));
        exercises.Add(E("Decline Bench Press", ExerciseType.WeightLifting, 3, 1));
        exercises.Add(E("Fly", ExerciseType.WeightLifting, 3, 1));
        exercises.Add(E("Incline Fly", ExerciseType.WeightLifting, 3, 1));
        exercises.Add(E("Pullover", ExerciseType.WeightLifting, 3, 1));
        exercises.Add(E("Squeeze Press", ExerciseType.WeightLifting, 3, 1));

        exercises.Add(E("Row", ExerciseType.WeightLifting, 3, 2));
        exercises.Add(E("Bent Over Row", ExerciseType.WeightLifting, 3, 2));
        exercises.Add(E("Reverse Fly", ExerciseType.WeightLifting, 3, 2));
        exercises.Add(E("Pullover Row", ExerciseType.WeightLifting, 3, 2));
        exercises.Add(E("Renegade Row", ExerciseType.WeightLifting, 3, 2));
        exercises.Add(E("Shrug", ExerciseType.WeightLifting, 3, 2));
        exercises.Add(E("Chest Supported Row", ExerciseType.WeightLifting, 3, 2));

        exercises.Add(E("Shoulder Press", ExerciseType.WeightLifting, 3, 3));
        exercises.Add(E("Arnold Press", ExerciseType.WeightLifting, 3, 3));
        exercises.Add(E("Lateral Raise", ExerciseType.WeightLifting, 3, 3));
        exercises.Add(E("Front Raise", ExerciseType.WeightLifting, 3, 3));
        exercises.Add(E("Rear Delt Fly", ExerciseType.WeightLifting, 3, 3));
        exercises.Add(E("Upright Row", ExerciseType.WeightLifting, 3, 3));
        exercises.Add(E("Y Raise", ExerciseType.WeightLifting, 3, 3));
        exercises.Add(E("Scott Press", ExerciseType.WeightLifting, 3, 3));

        exercises.Add(E("Bicep Curl", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Hammer Curl", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Concentration Curl", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Incline Curl", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Preacher Curl", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Tricep Extension", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Overhead Tricep Extension", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Tricep Kickback", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Zottman Curl", ExerciseType.WeightLifting, 3, 4));
        exercises.Add(E("Wrist Curl", ExerciseType.WeightLifting, 3, 4));

        exercises.Add(E("Squat", ExerciseType.WeightLifting, 3, 5));
        exercises.Add(E("Goblet Squat", ExerciseType.WeightLifting, 3, 5));
        exercises.Add(E("Lunge", ExerciseType.WeightLifting, 3, 5));
        exercises.Add(E("Walking Lunge", ExerciseType.WeightLifting, 3, 5));
        exercises.Add(E("Romanian Deadlift", ExerciseType.WeightLifting, 3, 5));
        exercises.Add(E("Step Up", ExerciseType.WeightLifting, 3, 5));
        exercises.Add(E("Bulgarian Split Squat", ExerciseType.WeightLifting, 3, 5));
        exercises.Add(E("Calf Raise", ExerciseType.WeightLifting, 3, 5));
        exercises.Add(E("Sumo Squat", ExerciseType.WeightLifting, 3, 5));

        exercises.Add(E("Russian Twist", ExerciseType.WeightLifting, 3, 6));
        exercises.Add(E("Side Bend", ExerciseType.WeightLifting, 3, 6));
        exercises.Add(E("Woodchop", ExerciseType.WeightLifting, 3, 6));

        exercises.Add(E("Hip Thrust", ExerciseType.WeightLifting, 3, 7));
        exercises.Add(E("Glute Bridge", ExerciseType.WeightLifting, 3, 7));
        exercises.Add(E("Sumo Deadlift", ExerciseType.WeightLifting, 3, 7));
        exercises.Add(E("Frog Pump", ExerciseType.WeightLifting, 3, 7));

        exercises.Add(E("Chest Fly", ExerciseType.WeightLifting, 11, 1));
        exercises.Add(E("Low Chest Fly", ExerciseType.WeightLifting, 11, 1));
        exercises.Add(E("High Chest Fly", ExerciseType.WeightLifting, 11, 1));
        exercises.Add(E("Crossover", ExerciseType.WeightLifting, 11, 1));

        exercises.Add(E("Seated Row", ExerciseType.WeightLifting, 11, 2));
        exercises.Add(E("Lat Pulldown", ExerciseType.WeightLifting, 11, 2));
        exercises.Add(E("Close Grip Pulldown", ExerciseType.WeightLifting, 11, 2));
        exercises.Add(E("Straight Arm Pulldown", ExerciseType.WeightLifting, 11, 2));
        exercises.Add(E("Face Pull", ExerciseType.WeightLifting, 11, 2));
        exercises.Add(E("Single Arm Row", ExerciseType.WeightLifting, 11, 2));
        exercises.Add(E("Wide Grip Pulldown", ExerciseType.WeightLifting, 11, 2));
        exercises.Add(E("Reverse Grip Pulldown", ExerciseType.WeightLifting, 11, 2));

        exercises.Add(E("Lateral Raise", ExerciseType.WeightLifting, 11, 3));
        exercises.Add(E("Front Raise", ExerciseType.WeightLifting, 11, 3));
        exercises.Add(E("Rear Delt Fly", ExerciseType.WeightLifting, 11, 3));
        exercises.Add(E("Upright Row", ExerciseType.WeightLifting, 11, 3));
        exercises.Add(E("External Rotation", ExerciseType.WeightLifting, 11, 3));

        exercises.Add(E("Bicep Curl", ExerciseType.WeightLifting, 11, 4));
        exercises.Add(E("Hammer Curl", ExerciseType.WeightLifting, 11, 4));
        exercises.Add(E("Overhead Curl", ExerciseType.WeightLifting, 11, 4));
        exercises.Add(E("Tricep Pushdown", ExerciseType.WeightLifting, 11, 4));
        exercises.Add(E("Rope Pushdown", ExerciseType.WeightLifting, 11, 4));
        exercises.Add(E("Overhead Tricep Extension", ExerciseType.WeightLifting, 11, 4));
        exercises.Add(E("Reverse Curl", ExerciseType.WeightLifting, 11, 4));
        exercises.Add(E("Single Arm Curl", ExerciseType.WeightLifting, 11, 4));

        exercises.Add(E("Pull Through", ExerciseType.WeightLifting, 11, 5));
        exercises.Add(E("Leg Extension", ExerciseType.WeightLifting, 11, 5));

        exercises.Add(E("Crunch", ExerciseType.WeightLifting, 11, 6));
        exercises.Add(E("Woodchop", ExerciseType.WeightLifting, 11, 6));
        exercises.Add(E("Pallof Press", ExerciseType.WeightLifting, 11, 6));
        exercises.Add(E("Reverse Crunch", ExerciseType.WeightLifting, 11, 6));

        exercises.Add(E("Kickback", ExerciseType.WeightLifting, 11, 7));
        exercises.Add(E("Hip Abduction", ExerciseType.WeightLifting, 11, 7));
        exercises.Add(E("Hip Adduction", ExerciseType.WeightLifting, 11, 7));

        exercises.Add(E("Chest Press", ExerciseType.WeightLifting, 4, 1));
        exercises.Add(E("Incline Chest Press", ExerciseType.WeightLifting, 4, 1));
        exercises.Add(E("Pec Deck Fly", ExerciseType.WeightLifting, 4, 1));

        exercises.Add(E("Seated Row", ExerciseType.WeightLifting, 4, 2));
        exercises.Add(E("Lat Pulldown", ExerciseType.WeightLifting, 4, 2));
        exercises.Add(E("Assisted Pull Up", ExerciseType.WeightLifting, 4, 2));
        exercises.Add(E("Reverse Fly", ExerciseType.WeightLifting, 4, 2));
        exercises.Add(E("T-Bar Row", ExerciseType.WeightLifting, 4, 2));

        exercises.Add(E("Shoulder Press", ExerciseType.WeightLifting, 4, 3));
        exercises.Add(E("Lateral Raise", ExerciseType.WeightLifting, 4, 3));
        exercises.Add(E("Rear Delt Fly", ExerciseType.WeightLifting, 4, 3));

        exercises.Add(E("Bicep Curl", ExerciseType.WeightLifting, 4, 4));
        exercises.Add(E("Tricep Extension", ExerciseType.WeightLifting, 4, 4));
        exercises.Add(E("Preacher Curl", ExerciseType.WeightLifting, 4, 4));
        exercises.Add(E("Tricep Dip", ExerciseType.WeightLifting, 4, 4));

        exercises.Add(E("Leg Press", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("Hack Squat", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("Leg Extension", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("Leg Curl", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("Seated Leg Curl", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("Standing Calf Raise", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("Seated Calf Raise", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("Smith Squat", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("Pendulum Squat", ExerciseType.WeightLifting, 4, 5));
        exercises.Add(E("V Squat", ExerciseType.WeightLifting, 4, 5));

        exercises.Add(E("Ab Crunch", ExerciseType.WeightLifting, 4, 6));
        exercises.Add(E("Torso Rotation", ExerciseType.WeightLifting, 4, 6));

        exercises.Add(E("Hip Thrust", ExerciseType.WeightLifting, 4, 7));
        exercises.Add(E("Glute Kickback", ExerciseType.WeightLifting, 4, 7));
        exercises.Add(E("Hip Abduction", ExerciseType.WeightLifting, 4, 7));
        exercises.Add(E("Hip Adduction", ExerciseType.WeightLifting, 4, 7));

        exercises.Add(E("Smith Machine Squat", ExerciseType.WeightLifting, 4, 8));
        exercises.Add(E("Smith Machine Bench Press", ExerciseType.WeightLifting, 4, 8));

        exercises.Add(E("Push Up", ExerciseType.BodyWeight, 6, 1));
        exercises.Add(E("Diamond Push Up", ExerciseType.BodyWeight, 6, 1));
        exercises.Add(E("Wide Push Up", ExerciseType.BodyWeight, 6, 1));
        exercises.Add(E("Decline Push Up", ExerciseType.BodyWeight, 6, 1));
        exercises.Add(E("Dip", ExerciseType.BodyWeight, 6, 1));

        exercises.Add(E("Pull Up", ExerciseType.BodyWeight, 6, 2));
        exercises.Add(E("Chin Up", ExerciseType.BodyWeight, 6, 2));
        exercises.Add(E("Wide Grip Pull Up", ExerciseType.BodyWeight, 6, 2));
        exercises.Add(E("Inverted Row", ExerciseType.BodyWeight, 6, 2));
        exercises.Add(E("Neutral Grip Pull Up", ExerciseType.BodyWeight, 6, 2));

        exercises.Add(E("Pike Push Up", ExerciseType.BodyWeight, 6, 3));
        exercises.Add(E("Handstand Push Up", ExerciseType.BodyWeight, 6, 3));

        exercises.Add(E("Tricep Dip", ExerciseType.BodyWeight, 6, 4));
        exercises.Add(E("Bench Dip", ExerciseType.BodyWeight, 6, 4));
        exercises.Add(E("Close Grip Push Up", ExerciseType.BodyWeight, 6, 4));

        exercises.Add(E("Squat", ExerciseType.BodyWeight, 6, 5));
        exercises.Add(E("Lunge", ExerciseType.BodyWeight, 6, 5));
        exercises.Add(E("Pistol Squat", ExerciseType.BodyWeight, 6, 5));
        exercises.Add(E("Jump Squat", ExerciseType.BodyWeight, 6, 5));
        exercises.Add(E("Wall Sit", ExerciseType.BodyWeight, 6, 5));
        exercises.Add(E("Calf Raise", ExerciseType.BodyWeight, 6, 5));
        exercises.Add(E("Box Jump", ExerciseType.BodyWeight, 6, 5));

        exercises.Add(E("Crunch", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Sit Up", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Plank", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Side Plank", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Leg Raise", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Hanging Leg Raise", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Hanging Knee Raise", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Mountain Climber", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Bicycle Crunch", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Flutter Kicks", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Russian Twist", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("V Up", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Dead Bug", ExerciseType.BodyWeight, 6, 6));
        exercises.Add(E("Ab Wheel Rollout", ExerciseType.BodyWeight, 6, 6));

        exercises.Add(E("Glute Bridge", ExerciseType.BodyWeight, 6, 7));
        exercises.Add(E("Single Leg Glute Bridge", ExerciseType.BodyWeight, 6, 7));
        exercises.Add(E("Donkey Kick", ExerciseType.BodyWeight, 6, 7));
        exercises.Add(E("Fire Hydrant", ExerciseType.BodyWeight, 6, 7));

        exercises.Add(E("Burpee", ExerciseType.BodyWeight, 6, 8));
        exercises.Add(E("Bear Crawl", ExerciseType.BodyWeight, 6, 8));

        exercises.Add(E("Pull Up", ExerciseType.WeightLifting, 8, 2));
        exercises.Add(E("Chin Up", ExerciseType.WeightLifting, 8, 2));
        exercises.Add(E("Dip", ExerciseType.WeightLifting, 8, 1));
        exercises.Add(E("Tricep Dip", ExerciseType.WeightLifting, 8, 4));
        exercises.Add(E("Pistol Squat", ExerciseType.WeightLifting, 8, 5));

        exercises.Add(E("Clean", ExerciseType.WeightLifting, 10, 8));
        exercises.Add(E("Clean and Jerk", ExerciseType.WeightLifting, 10, 8));
        exercises.Add(E("Snatch", ExerciseType.WeightLifting, 10, 8));
        exercises.Add(E("Power Clean", ExerciseType.WeightLifting, 10, 8));
        exercises.Add(E("Hang Clean", ExerciseType.WeightLifting, 10, 8));
        exercises.Add(E("Push Press", ExerciseType.WeightLifting, 10, 3));
        exercises.Add(E("Clean Pull", ExerciseType.WeightLifting, 10, 2));
        exercises.Add(E("Snatch Pull", ExerciseType.WeightLifting, 10, 2));
        exercises.Add(E("Thruster", ExerciseType.WeightLifting, 10, 8));
        exercises.Add(E("Overhead Squat", ExerciseType.WeightLifting, 10, 8));

        exercises.Add(E("Running", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Treadmill Running", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Cycling", ExerciseType.Cardio, 1, 5));
        exercises.Add(E("Stationary Bike", ExerciseType.Cardio, 1, 5));
        exercises.Add(E("Elliptical", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Rowing Machine", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Stair Climber", ExerciseType.Cardio, 1, 5));
        exercises.Add(E("Jump Rope", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Swimming", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Walking", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Incline Walking", ExerciseType.Cardio, 1, 5));
        exercises.Add(E("Battle Ropes", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Assault Bike", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Ski Erg", ExerciseType.Cardio, 1, 8));
        exercises.Add(E("Sprinting", ExerciseType.Cardio, 1, 5));

        exercises.Add(E("Yoga", ExerciseType.Cardio, 7, 8));
        exercises.Add(E("Pilates", ExerciseType.Cardio, 7, 6));
        exercises.Add(E("Foam Rolling", ExerciseType.Cardio, 7, 8));
        exercises.Add(E("Sauna", ExerciseType.Cardio, 7, 8));
        exercises.Add(E("Hiking", ExerciseType.Cardio, 7, 5));
        exercises.Add(E("Active Recovery Walk", ExerciseType.Cardio, 7, 8));

        exercises.Add(E("Hamstring Stretch", ExerciseType.Stretching, 9, 5));
        exercises.Add(E("Quad Stretch", ExerciseType.Stretching, 9, 5));
        exercises.Add(E("Hip Flexor Stretch", ExerciseType.Stretching, 9, 5));
        exercises.Add(E("Chest Stretch", ExerciseType.Stretching, 9, 1));
        exercises.Add(E("Shoulder Stretch", ExerciseType.Stretching, 9, 3));
        exercises.Add(E("Tricep Stretch", ExerciseType.Stretching, 9, 4));
        exercises.Add(E("Lat Stretch", ExerciseType.Stretching, 9, 2));
        exercises.Add(E("Calf Stretch", ExerciseType.Stretching, 9, 5));
        exercises.Add(E("Glute Stretch", ExerciseType.Stretching, 9, 7));
        exercises.Add(E("Neck Stretch", ExerciseType.Stretching, 9, 3));
        exercises.Add(E("Cat Cow Stretch", ExerciseType.Stretching, 9, 6));
        exercises.Add(E("Child's Pose", ExerciseType.Stretching, 9, 2));
        exercises.Add(E("Pigeon Stretch", ExerciseType.Stretching, 9, 7));
        exercises.Add(E("Seated Forward Fold", ExerciseType.Stretching, 9, 5));
        exercises.Add(E("Spinal Twist", ExerciseType.Stretching, 9, 6));

        return exercises.AsReadOnly();
    }
}
