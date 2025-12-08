using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTrackerApi.Models;

namespace WorkoutTrackerApi.Data.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.Property(p => p.Notes)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.HasIndex(p => p.Name);
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder
            .HasOne(w => w.User)
            .WithMany(u => u.Workouts)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}