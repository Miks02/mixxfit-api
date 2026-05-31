using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Entities.FitnessProfiles;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class FitnessProfileConfiguration : IEntityTypeConfiguration<FitnessProfile>
{
    public void Configure(EntityTypeBuilder<FitnessProfile> builder)
    {
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .HasOne(f => f.User)
            .WithOne(u => u.FitnessProfile)
            .HasForeignKey<FitnessProfile>(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        builder
            .HasMany(u => u.Workouts)
            .WithOne(w => w.FitnessProfile)
            .HasForeignKey(w => w.FitnessProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(f => f.WeightEntries)
            .WithOne(w => w.FitnessProfile)
            .HasForeignKey(w => w.FitnessProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.Exercises)
            .WithOne(e => e.FitnessProfile)
            .HasForeignKey(e => e.FitnessProfileId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
