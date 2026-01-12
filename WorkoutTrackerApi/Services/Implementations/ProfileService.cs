using Microsoft.EntityFrameworkCore;
using WorkoutTrackerApi.Data;
using WorkoutTrackerApi.DTO.User;
using WorkoutTrackerApi.Services.Interfaces;

namespace WorkoutTrackerApi.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IWorkoutService _workoutService;

        public ProfileService(AppDbContext context, IUserService userService, IWorkoutService workoutService)
        {
            _context = context;
            _userService = userService;
            _workoutService = workoutService;
        }

        public async Task<ProfilePageDto> GetUserProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrEmpty(userId)) 
                throw new InvalidOperationException("CRITICAL ERROR: User Id is null");

            var userDetails = await _userService.GetUserDetailsAsync(userId, cancellationToken);
            var recentWorkouts = await _workoutService.GetRecentWorkoutsAsync(userId, 10, cancellationToken);
            var workoutStreak = await _workoutService.CalculateWorkoutStreakAsync(userId, cancellationToken);
            var dailyCalorieGoal = await GetDailyCalorieGoalAsync(userId, cancellationToken);

            return new ProfilePageDto
            {
                UserDetails = userDetails,
                RecentWorkouts = recentWorkouts,
                WorkoutStreak = workoutStreak,
                DailyCalorieGoal = dailyCalorieGoal
            };

        }

        private async Task<int?> GetDailyCalorieGoalAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Select(u => u.DailyCalorieGoal)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
