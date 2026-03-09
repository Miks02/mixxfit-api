using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class SetEntryConfiguration : IEntityTypeConfiguration<SetEntry>
{
    public void Configure(EntityTypeBuilder<SetEntry> builder)
    {
        builder.ToTable(entries => entries.HasCheckConstraint($"CK_{nameof(SetEntry)}s_{nameof(SetEntry.Reps)}_Positive", "\"Reps\" > 0"));
        builder.ToTable(entries => entries.HasCheckConstraint($"CK_{nameof(SetEntry)}s_{nameof(SetEntry.WeightKg)}_Positive", "\"WeightKg\" > 0"));
    }
}