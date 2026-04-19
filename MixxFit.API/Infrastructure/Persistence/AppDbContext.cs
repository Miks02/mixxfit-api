using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities;

namespace MixxFit.API.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<FitnessProfile> FitnessProfiles { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<ExerciseEntry> ExerciseEntries { get; set; }
    public DbSet<SetEntry> SetEntries { get; set; }
    public DbSet<WeightEntry> WeightEntries { get; set; }
    public DbSet<CalorieEntry> CalorieEntries { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseCategory> ExerciseCategories { get; set; }
    public DbSet<MuscleGroup> MuscleGroups { get; set; }
    public DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }
    public DbSet<WorkoutTemplateExercise> WorkoutTemplateExercises { get; set; }
    
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