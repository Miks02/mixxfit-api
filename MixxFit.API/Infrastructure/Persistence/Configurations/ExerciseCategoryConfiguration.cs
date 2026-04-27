using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Entities.ExerciseCategories;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class ExerciseCategoryConfiguration : IEntityTypeConfiguration<ExerciseCategory>
{
    public void Configure(EntityTypeBuilder<ExerciseCategory> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.HasMany(p => p.Exercises)
            .WithOne(e => e.ExerciseCategory)
            .HasForeignKey(e => e.ExerciseCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(p => p.Name)
            .IsUnique();
        
        builder.HasData(
            new ExerciseCategory { Id = 1, Name = "Cardio" },
            new ExerciseCategory { Id = 2, Name = "Barbell" },
            new ExerciseCategory { Id = 3, Name = "Dumbbell" },
            new ExerciseCategory { Id = 4, Name = "Machine" },
            new ExerciseCategory { Id = 5, Name = "Other" },
            new ExerciseCategory { Id = 6, Name = "Bodyweight" },
            new ExerciseCategory { Id = 7, Name = "Duration" },
            new ExerciseCategory { Id = 8, Name = "Assisted Bodyweight" },
            new ExerciseCategory { Id = 9, Name = "Stretching" },
            new ExerciseCategory { Id = 10, Name = "Olympic" },
            new ExerciseCategory { Id = 11, Name = "Cable" }
            );
    }
}