using MixxFit.API.DTO.Global;
using MixxFit.API.DTO.Workout;
using MixxFit.API.Services.Results;

namespace MixxFit.API.Services.Interfaces;

public interface IWorkoutService
{
    Task<WorkoutPageDto> GetWorkoutsPagedAsync(QueryParams queryParams, string userId, CancellationToken cancellationToken = default);
    Task<PagedResult<WorkoutListItemDto>> GetWorkoutsByQueryParamsAsync(QueryParams queryParams, string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkoutListItemDto>> GetRecentWorkoutsAsync(string userId, int itemsToTake, CancellationToken cancellationToken = default);
    Task<Result<WorkoutDetailsDto>> GetWorkoutByIdAsync(int id, string? userId, CancellationToken cancellationToken = default);
    Task<DateTime?> GetLastWorkoutAsync(string userId, CancellationToken cancellationToken = default);
    Task<WorkoutsPerMonthDto> GetWorkoutCountsByMonthAsync(string userId, int? year);

    Task<int?> CalculateWorkoutStreakAsync(string userId, CancellationToken cancellationToken = default);
   
    Task<Result<WorkoutDetailsDto>> AddWorkoutAsync(WorkoutCreateRequest request, string userId, CancellationToken cancellationToken = default);
    Task<Result> DeleteWorkoutAsync(int id, string userId, CancellationToken cancellationToken = default);
}