using VitalOps.API.DTO.Dashboard;

namespace VitalOps.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> LoadDashboardAsync(string userId, CancellationToken cancellationToken);
    }
}
