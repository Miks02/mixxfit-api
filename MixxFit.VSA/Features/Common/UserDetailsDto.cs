using MixxFit.VSA.Domain.Enums;

namespace MixxFit.VSA.Features.Common;

public record UserDetailsDto(
    string FullName,
    string UserName,
    string Email,
    string? ImagePath,
    double? CurrentWeight,
    double? TargetWeight,
    double? Height,
    DateTime? DateOfBirth,
    AccountStatus AccountStatus,
    Gender? Gender
)
{
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

