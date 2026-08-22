using MixxFit.API.Domain.Entities.Users;

namespace MixxFit.API.Domain.Entities.RefreshTokens;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string TokenHash { get; set; } = null!;
    public string CreatedByIp { get; set; } = null!;
    public string? ReplacedByTokenHash { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}