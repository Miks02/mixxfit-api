using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(p => new { p.Name, p.UserId })
            .IsUnique();
        
        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasFilter("\"UserId\" IS NULL");
        
        builder.HasOne(p => p.User)
            .WithMany(u => u.Exercises)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(p => p.ExerciseCategory)
            .WithMany(p => p.Exercises)
            .HasForeignKey(p => p.ExerciseCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(p => p.MuscleGroup)
            .WithMany(p => p.Exercises)
            .HasForeignKey(p => p.MuscleGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}