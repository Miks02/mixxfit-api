using MixxFit.VSA.Domain.Enums;

namespace MixxFit.VSA.Features.Auth.Login;

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