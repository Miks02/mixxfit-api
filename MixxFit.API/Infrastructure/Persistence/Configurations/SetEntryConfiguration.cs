using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Entities.SetEntries;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class SetEntryConfiguration : IEntityTypeConfiguration<SetEntry>
{
    public void Configure(EntityTypeBuilder<SetEntry> builder)
    {
        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                $"CK_SetEntries_{nameof(SetEntry.Reps)}_Positive",
                $"\"{nameof(SetEntry.Reps)}\" IS NULL OR \"{nameof(SetEntry.Reps)}\" > 0");

            t.HasCheckConstraint(
                $"CK_SetEntries_{nameof(SetEntry.Weight)}_Positive",
                $"\"{nameof(SetEntry.Weight)}\" IS NULL OR \"{nameof(SetEntry.Weight)}\" > 0");

            t.HasCheckConstraint(
                $"CK_SetEntries_{nameof(SetEntry.Distance)}_Positive",
                $"\"{nameof(SetEntry.Distance)}\" IS NULL OR \"{nameof(SetEntry.Distance)}\" > 0");

            t.HasCheckConstraint(
                $"CK_SetEntries_{nameof(SetEntry.DurationSeconds)}_Positive",
                $"\"{nameof(SetEntry.DurationSeconds)}\" IS NULL OR \"{nameof(SetEntry.DurationSeconds)}\" > 0");
        });

        builder.Property(x => x.Weight).HasColumnType("decimal(6,2)");
        builder.Property(x => x.Distance).HasColumnType("decimal(6,2)");
    }
}