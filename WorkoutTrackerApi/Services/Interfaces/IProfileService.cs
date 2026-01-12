using WorkoutTrackerApi.DTO.User;

namespace WorkoutTrackerApi.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfilePageDto> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default);
    }
}
