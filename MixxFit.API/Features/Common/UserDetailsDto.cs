using MixxFit.API.Domain.Enums;

namespace MixxFit.API.Features.Common;

public record UserDetailsDto
{
    public string FullName { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? ImagePath { get; init; }
    public decimal? CurrentWeight { get; init; }
    public double? TargetWeight { get; init; }
    public double? Height { get; init; }
    public double? DailyCalorieGoal { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public AccountStatus AccountStatus { get; init; }
    public Gender? Gender { get; init; }
    
    public int? Age => DateOfBirth.HasValue ? CalculateAge(DateOfBirth.Value) : null;
    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
            age--;
        return age;
    }
}

