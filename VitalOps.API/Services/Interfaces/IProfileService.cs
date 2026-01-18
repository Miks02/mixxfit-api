using VitalOps.API.DTO.User;

namespace VitalOps.API.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfilePageDto> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default);
    }
}
