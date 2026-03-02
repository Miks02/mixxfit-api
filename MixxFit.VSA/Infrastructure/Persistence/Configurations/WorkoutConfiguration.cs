using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.VSA.Domain.Entities;

namespace MixxFit.VSA.Infrastructure.Persistence.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.Property(p => p.Notes)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.HasIndex(p => p.Name);

        builder.HasIndex(p => p.WorkoutDate);
        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.CreatedAt);
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .HasOne(w => w.User)
            .WithMany(u => u.Workouts)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}