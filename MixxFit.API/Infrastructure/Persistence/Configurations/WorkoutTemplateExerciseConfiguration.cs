using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;

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
    }
}