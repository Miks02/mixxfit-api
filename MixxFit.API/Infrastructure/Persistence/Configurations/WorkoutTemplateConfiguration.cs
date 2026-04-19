using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class WorkoutTemplateConfiguration : IEntityTypeConfiguration<WorkoutTemplate>
{
    public void Configure(EntityTypeBuilder<WorkoutTemplate> builder)
    {
            builder.Property(wt => wt.Name)
                .HasMaxLength(100)
                .IsRequired();
            
            builder.HasIndex(wt => new { wt.FitnessProfileId, wt.Name })
                .IsUnique();
    
            builder.Property(wt => wt.Notes)
                .HasMaxLength(200)
                .IsRequired(false);
        
            
            builder.HasOne(wt => wt.FitnessProfile)
                .WithMany(fp => fp.WorkoutTemplates)
                .HasForeignKey(wt => wt.FitnessProfileId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}