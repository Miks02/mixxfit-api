using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Entities.ExerciseEntries;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class ExerciseEntryConfiguration : IEntityTypeConfiguration<ExerciseEntry>
{
    public void Configure(EntityTypeBuilder<ExerciseEntry> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.HasIndex(p => p.Name);

        builder
            .HasMany(e => e.Sets)
            .WithOne(s => s.ExerciseEntry)
            .HasForeignKey(s => s.ExerciseEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(ee => ee.Exercise)
            .WithMany(e => e.ExerciseEntries)
            .HasForeignKey(ee => ee.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}