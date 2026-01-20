using VitalOps.API.DTO.ExerciseEntry;
using VitalOps.API.DTO.SetEntry;
using VitalOps.API.DTO.Workout;
using VitalOps.API.Enums;
using VitalOps.API.Extensions;
using VitalOps.API.Models;

namespace VitalOps.API.Mappers
{
    public static class WorkoutMapper
    {
        public static WorkoutListItemDto ToWorkoutListItemDto(this Workout workout)
        {
            return new WorkoutListItemDto()
            {
                Id = workout.Id,
                Name = workout.Name,
                ExerciseCount = workout.ExerciseEntries.Count,
                SetCount = workout.ExerciseEntries.Sum(e => e.Sets.Count),
                WorkoutDate = workout.WorkoutDate,
                HasCardio = workout.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.Cardio),
                HasWeights = workout.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.WeightLifting),
                HasBodyWeight = workout.ExerciseEntries.Any(e => e.ExerciseType == ExerciseType.BodyWeight)
            };
        }

        public static WorkoutDetailsDto ToWorkoutDetailsDto(this Workout workout)
        {
            return new WorkoutDetailsDto()
            {
                Id = workout.Id,
                Name = workout.Name,
                Notes = workout.Notes,
                UserId = workout.UserId,
                CreatedAt = workout.CreatedAt,
                WorkoutDate = workout.WorkoutDate,
                Exercises = workout.ExerciseEntries.Select(e => new ExerciseEntryDto()
                {
                    Id = e.Id,
                    Name = e.Name,
                    ExerciseType = e.ExerciseType,
                    CardioType = e.CardioType,
                    AvgHeartRate = e.AvgHeartRate,
                    CaloriesBurned = e.CaloriesBurned,
                    DistanceKm = e.DistanceKm,
                    DurationMinutes = e.Duration.ToIntegerFromNullableMinutes(),
                    DurationSeconds = e.Duration.ToIntegerFromNullableSeconds(),
                    Sets = e.Sets.Select(s => new SetEntryDto()
                    {
                        Reps = s.Reps,
                        WeightKg = s.WeightKg
                    }).ToList()
                }).ToList()
            };
        }
    }
}
