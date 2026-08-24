using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.ExerciseCategories;
using MixxFit.API.Domain.Entities.ExerciseEntries;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.MuscleGroups;
using MixxFit.API.Domain.Entities.RefreshTokens;
using MixxFit.API.Domain.Entities.SetEntries;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Entities.WorkoutTemplateExercises;
using MixxFit.API.Domain.Entities.WorkoutTemplates;

namespace MixxFit.API.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<FitnessProfile> FitnessProfiles { get; set; }
    public virtual DbSet<Workout> Workouts { get; set; }
    public virtual DbSet<ExerciseEntry> ExerciseEntries { get; set; }
    public virtual DbSet<SetEntry> SetEntries { get; set; }
    public virtual DbSet<WeightEntry> WeightEntries { get; set; }
    public virtual DbSet<Exercise> Exercises { get; set; }
    public virtual DbSet<ExerciseCategory> ExerciseCategories { get; set; }
    public virtual DbSet<MuscleGroup> MuscleGroups { get; set; }
    public virtual DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }
    public virtual DbSet<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasCollation("my_case_insensitive", locale: "en-u-ks-level2", provider: "icu", deterministic: false);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");

        builder.Entity<IdentityRole>().ToTable("Roles");

        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

    }
}