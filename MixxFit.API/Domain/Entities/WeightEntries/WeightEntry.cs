using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;

namespace MixxFit.API.Domain.Entities.WeightEntries
{
    public class WeightEntry
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        public double Weight { get; set; }
        public TimeSpan Time { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int FitnessProfileId { get; set; }
        public FitnessProfile? FitnessProfile { get; set; }
    }
}
