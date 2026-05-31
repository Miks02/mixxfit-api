using MixxFit.API.Domain.Entities.FitnessProfiles;

namespace MixxFit.API.Domain.Entities.WeightEntries
{
    public class WeightEntry
    {
        public int Id { get; set; }

        public int FitnessProfileId { get; set; }
        public FitnessProfile? FitnessProfile { get; set; }

        public double Weight { get; set; }
        public TimeSpan Time { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
