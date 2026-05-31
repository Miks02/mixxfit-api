using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities.WeightEntries;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class WeightEntryConfiguration : IEntityTypeConfiguration<WeightEntry>
{
    public void Configure(EntityTypeBuilder<WeightEntry> builder)
    {
        builder.Property(x => x.Notes)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.HasIndex(x => x.Weight);
        builder.HasIndex(x => x.CreatedAt);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .HasOne(w => w.FitnessProfile)
            .WithMany(u => u.WeightEntries)
            .HasForeignKey(w => w.FitnessProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(entries => entries.HasCheckConstraint($"CK_{nameof(WeightEntry)}s_{nameof(WeightEntry.Weight)}_Positive", "\"Weight\" > 25"));
        builder.ToTable(entries => entries.HasCheckConstraint($"CK_{nameof(WeightEntry)}s_{nameof(WeightEntry.Weight)}_LessThan400", "\"Weight\" < 400"));


    }
}
