using MixxFit.API.Domain.Entities.Users;

namespace MixxFit.API.Domain.Entities.Admins;

public class Admin
{
    public string AdminId { get; set; } = null!;
    public User User { get; set; } = null!;
}