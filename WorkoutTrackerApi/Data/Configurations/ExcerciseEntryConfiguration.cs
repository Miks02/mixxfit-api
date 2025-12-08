using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkoutTrackerApi.Models;

namespace WorkoutTrackerApi.Data.Configurations;

public class ExerciseEntryConfiguration : IEntityTypeConfiguration<ExerciseEntry>
{
    public void Configure(EntityTypeBuilder<ExerciseEntry> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.HasIndex(p => p.Name);

        builder.ToTable(entries => entries.HasCheckConstraint($"CK_{nameof(ExerciseEntry)}s_{nameof(ExerciseEntry.DistanceKm)}_Positive", "DistanceKm > 0"));
        builder.ToTable(entries => entries.HasCheckConstraint($"CK_{nameof(ExerciseEntry)}s_{nameof(ExerciseEntry.AvgHeartRate)}_Positive", "AvgHeartRate > 0"));
        builder.ToTable(entries => entries.HasCheckConstraint($"CK_{nameof(ExerciseEntry)}s_{nameof(ExerciseEntry.CaloriesBurned)}_Positive", "CaloriesBurned > 0"));

        builder
            .HasMany(e => e.Sets)
            .WithOne(s => s.ExerciseEntry)
            .HasForeignKey(s => s.ExerciseEntryId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}