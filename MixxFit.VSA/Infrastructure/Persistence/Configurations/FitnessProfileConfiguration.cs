using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.VSA.Domain.Entities;

namespace MixxFit.VSA.Infrastructure.Persistence.Configurations;

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
        
    }
}