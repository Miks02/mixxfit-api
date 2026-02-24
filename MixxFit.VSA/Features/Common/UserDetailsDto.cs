using MixxFit.VSA.Domain.Enums;

namespace MixxFit.VSA.Features.Common;

public record UserDetailsDto(
    string FullName, 
    string Email, 
    string? ImagePath, 
    double? CurrentWeight,
    double? TargetWeight,
    double? Height,
    DateTime? DateOfBirth,
    AccountStatus AccountStatus,
    Gender? Gender
    );