using VitalOps.API.DTO.Global;

namespace VitalOps.API.DTO.Weight
{
    public class WeightSummaryDto
    {
        public WeightRecordDto FirstEntry { get; set; } = null!;
        public WeightRecordDto CurrentWeight { get; set; } = null!;
        public double Progress { get; set; }
        public IReadOnlyList<WeightRecordDto> WeightLogs { get; set; } = [];

    }
}
