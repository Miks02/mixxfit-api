namespace VitalOps.API.DTO.Weight
{
    public class WeightSummaryDto
    {
        public double CurrentWeight { get; set; }
        public double Progress { get; set; }
        public IReadOnlyList<WeightEntryDetailsDto> WeightEntries { get; set; } = new List<WeightEntryDetailsDto>();
    }
}
