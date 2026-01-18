using Microsoft.AspNetCore.Identity;
using VitalOps.API.Enums;

namespace VitalOps.API.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? ImagePath { get; set; }
    
    public string? RefreshToken { get; set; } 
    public DateTime? TokenExpDate { get; set; }
    
    public Gender? Gender { get; set; }
    public ActivityLevel? ActivityLevel { get; set; }
    
    public double? WeightKg { get; set; }
    public double? HeightCm { get; set; }
    public int? DailyCalorieGoal { get; set; }

    public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DateOfBirth { get; set; }

    public ICollection<Workout> Workouts { get; set; } = [];
    public ICollection<CalorieEntry> CalorieEntries { get; set; } = [];

}