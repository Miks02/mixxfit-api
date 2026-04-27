using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Entities.WorkoutTemplateExercises;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class WorkoutTemplateExerciseConfiguration : IEntityTypeConfiguration<WorkoutTemplateExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutTemplateExercise> builder)
    {
        builder.HasKey(wte => new { wte.WorkoutTemplateId, wte.ExerciseId });
        
        builder.Property(wte => wte.Order)
            .IsRequired();
        
        builder.HasOne(wte => wte.WorkoutTemplate)
            .WithMany(wt => wt.WorkoutTemplateExercises)
            .HasForeignKey(wte => wte.WorkoutTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wte => wte.Exercise)
            .WithMany(e => e.WorkoutTemplateExercises)
            .HasForeignKey(wte => wte.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(BuildWorkoutTemplateExercises());
    }

    private static IReadOnlyList<WorkoutTemplateExercise> BuildWorkoutTemplateExercises()
    {
        return new List<WorkoutTemplateExercise>
        {
            new() { WorkoutTemplateId = 1, ExerciseId = 25, Order = 1, SetCount = 5 },
            new() { WorkoutTemplateId = 1, ExerciseId = 1, Order = 2, SetCount = 4 },
            new() { WorkoutTemplateId = 1, ExerciseId = 7, Order = 3, SetCount = 4 },
            new() { WorkoutTemplateId = 1, ExerciseId = 13, Order = 4, SetCount = 3 },
            new() { WorkoutTemplateId = 1, ExerciseId = 28, Order = 5, SetCount = 3 },
            new() { WorkoutTemplateId = 1, ExerciseId = 34, Order = 6, SetCount = 2 },

            new() { WorkoutTemplateId = 2, ExerciseId = 10, Order = 1, SetCount = 5 },
            new() { WorkoutTemplateId = 2, ExerciseId = 2, Order = 2, SetCount = 4 },
            new() { WorkoutTemplateId = 2, ExerciseId = 8, Order = 3, SetCount = 4 },
            new() { WorkoutTemplateId = 2, ExerciseId = 26, Order = 4, SetCount = 3 },
            new() { WorkoutTemplateId = 2, ExerciseId = 36, Order = 5, SetCount = 3 },
            new() { WorkoutTemplateId = 2, ExerciseId = 18, Order = 6, SetCount = 2 },

            new() { WorkoutTemplateId = 3, ExerciseId = 1, Order = 1, SetCount = 4 },
            new() { WorkoutTemplateId = 3, ExerciseId = 2, Order = 2, SetCount = 4 },
            new() { WorkoutTemplateId = 3, ExerciseId = 4, Order = 3, SetCount = 3 },
            new() { WorkoutTemplateId = 3, ExerciseId = 13, Order = 4, SetCount = 3 },
            new() { WorkoutTemplateId = 3, ExerciseId = 16, Order = 5, SetCount = 2 },
            new() { WorkoutTemplateId = 3, ExerciseId = 21, Order = 6, SetCount = 3 },

            new() { WorkoutTemplateId = 4, ExerciseId = 7, Order = 1, SetCount = 4 },
            new() { WorkoutTemplateId = 4, ExerciseId = 8, Order = 2, SetCount = 4 },
            new() { WorkoutTemplateId = 4, ExerciseId = 9, Order = 3, SetCount = 3 },
            new() { WorkoutTemplateId = 4, ExerciseId = 10, Order = 4, SetCount = 4 },
            new() { WorkoutTemplateId = 4, ExerciseId = 11, Order = 5, SetCount = 3 },
            new() { WorkoutTemplateId = 4, ExerciseId = 23, Order = 6, SetCount = 2 },

            new() { WorkoutTemplateId = 5, ExerciseId = 25, Order = 1, SetCount = 4 },
            new() { WorkoutTemplateId = 5, ExerciseId = 26, Order = 2, SetCount = 3 },
            new() { WorkoutTemplateId = 5, ExerciseId = 27, Order = 3, SetCount = 3 },
            new() { WorkoutTemplateId = 5, ExerciseId = 28, Order = 4, SetCount = 4 },
            new() { WorkoutTemplateId = 5, ExerciseId = 30, Order = 5, SetCount = 3 },
            new() { WorkoutTemplateId = 5, ExerciseId = 34, Order = 6, SetCount = 2 },
            new() { WorkoutTemplateId = 5, ExerciseId = 35, Order = 7, SetCount = 2 },

            new() { WorkoutTemplateId = 6, ExerciseId = 10, Order = 1, SetCount = 5 },
            new() { WorkoutTemplateId = 6, ExerciseId = 11, Order = 2, SetCount = 4 },
            new() { WorkoutTemplateId = 6, ExerciseId = 28, Order = 3, SetCount = 4 },
            new() { WorkoutTemplateId = 6, ExerciseId = 31, Order = 4, SetCount = 3 },
            new() { WorkoutTemplateId = 6, ExerciseId = 36, Order = 5, SetCount = 3 },
            new() { WorkoutTemplateId = 6, ExerciseId = 37, Order = 6, SetCount = 2 },
            new() { WorkoutTemplateId = 6, ExerciseId = 38, Order = 7, SetCount = 3 }
        };
    }
}