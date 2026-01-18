namespace VitalOps.API.Services.Interfaces;

public interface ICurrentUserService
{
    string? UserId();
    string? UserName();
    bool IsAdmin();
}