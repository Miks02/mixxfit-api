using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class MuscleGroupConfiguration : IEntityTypeConfiguration<MuscleGroup>
{
    public void Configure(EntityTypeBuilder<MuscleGroup> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(p => p.Name)
            .IsUnique();
        
        builder.HasMany(p => p.Exercises)
            .WithOne(e => e.MuscleGroup)
            .HasForeignKey(e => e.MuscleGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}