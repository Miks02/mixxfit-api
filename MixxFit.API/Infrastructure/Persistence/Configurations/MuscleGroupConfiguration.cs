using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class MuscleGroupConfiguration : IEntityTypeConfiguration<MuscleGroup>
{
    public void Configure(EntityTypeBuilder<MuscleGroup> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(p => p.Name)
            .IsUnique();
        
        builder.HasMany(p => p.Exercises)
            .WithOne(e => e.MuscleGroup)
            .HasForeignKey(e => e.MuscleGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new MuscleGroup { Id = 1, Name = "Chest" },
            new MuscleGroup { Id = 2, Name = "Back" },
            new MuscleGroup { Id = 3, Name = "Shoulders" },
            new MuscleGroup { Id = 4, Name = "Arms" },
            new MuscleGroup { Id = 5, Name = "Legs" },
            new MuscleGroup { Id = 6, Name = "Core" },
            new MuscleGroup { Id = 7, Name = "Glutes" },
            new MuscleGroup { Id = 8, Name = "Full Body" },
            new MuscleGroup { Id = 9, Name = "Other" }
        );
    }
}