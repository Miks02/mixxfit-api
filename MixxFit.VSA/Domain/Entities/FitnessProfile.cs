using MixxFit.VSA.Domain.Enums;

namespace MixxFit.VSA.Domain.Entities;

public class FitnessProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    
    public Gender? Gender { get; set; }
    public double? Height { get; set; }
    public double? Weight { get; set; }
    public double? DailyCalorieGoal { get; set; }
    public double? TargetWeight { get; set; }
    public double? BMI { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
}