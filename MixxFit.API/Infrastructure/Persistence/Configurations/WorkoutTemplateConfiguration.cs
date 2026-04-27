using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Entities.WorkoutTemplates;

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
            
            builder.HasData(BuildWorkoutTemplates());
    }

    private IReadOnlyList<WorkoutTemplate> BuildWorkoutTemplates()
    {
        return new List<WorkoutTemplate>
        {
            new()
            {
                Id = 1,
                FitnessProfileId = null,
                Name = "Full Body Strength A",
                Notes = "Balanced full-body strength session focused on compound lifts with moderate volume and steady progression."
            },
            new()
            {
                Id = 2,
                FitnessProfileId = null,
                Name = "Full Body Strength B",
                Notes = "Alternate full-body day emphasizing posterior chain work, squat variation, and upper body strength."
            },
            new()
            {
                Id = 3,
                FitnessProfileId = null,
                Name = "Push Hypertrophy",
                Notes = "Upper-body push session built for chest, shoulders, and triceps hypertrophy with controlled tempo."
            },
            new()
            {
                Id = 4,
                FitnessProfileId = null,
                Name = "Pull Hypertrophy",
                Notes = "Upper-body pull session targeting back thickness, hinge strength, and biceps development."
            },
            new()
            {
                Id = 5,
                FitnessProfileId = null,
                Name = "Legs and Core",
                Notes = "Lower-body and core session centered on squat strength, posterior chain support, and trunk stability."
            },
            new()
            {
                Id = 6,
                FitnessProfileId = null,
                Name = "Posterior Chain Focus",
                Notes = "Hinge-dominant training day focused on glutes, hamstrings, and lower back resilience."
            }
        };

    }
}