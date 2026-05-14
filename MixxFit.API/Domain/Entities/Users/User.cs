using Microsoft.AspNetCore.Identity;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Domain.Entities.Users;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? ImagePath { get; set; }
    
    public string? RefreshToken { get; set; } 
    public DateTime? TokenExpDate { get; set; }

    public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public FitnessProfile FitnessProfile { get; set; } = null!;
    public int FitnessProfileId { get; set; }

    public ICollection<WeightEntry> WeightEntries { get; set; } = [];
    
}