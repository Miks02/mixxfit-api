using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerApi.DTO.Global;
using WorkoutTrackerApi.DTO.Workout;
using WorkoutTrackerApi.Extensions;
using WorkoutTrackerApi.Models;
using WorkoutTrackerApi.Services.Interfaces;

namespace WorkoutTrackerApi.Controllers
{
    [Authorize]
    [Route("api/workouts")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        
        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }


        [HttpGet("overview")]
        public async Task<ActionResult<WorkoutPageDto>> GetMyWorkoutsPage(
            [FromQuery] string sortBy = "newest", 
            [FromQuery] string searchBy = "", 
            [FromQuery] DateTime? date = null, 
            [FromQuery] int page = 1 )
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var queryParams = new QueryParams(page, searchBy, sortBy, date);
            
            var getWorkoutsResult = await _workoutService.GetUserWorkoutsPagedAsync(queryParams, userId!);

            return Ok(getWorkoutsResult);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<WorkoutListItemDto>>> GetMyWorkoutsByQueryParams(
            [FromQuery] string sortBy = "newest", 
            [FromQuery] string searchBy = "", 
            [FromQuery] DateTime? date = null, 
            [FromQuery] int page = 1)
        {
            var queryParams = new QueryParams(page, searchBy, sortBy, date);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var workouts = await _workoutService.GetUserWorkoutsByQueryParamsAsync(queryParams, userId!);

            return Ok(workouts);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetWorkout([FromRoute] int id)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id);
            
            return workout.ToActionResult();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteWorkout([FromRoute] int id)
        {
            var workoutDeleteResult = await _workoutService.DeleteWorkoutAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            return workoutDeleteResult.ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult> AddWorkout([FromBody] WorkoutCreateRequest request)
        {

            request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var addResult = await _workoutService.AddWorkoutAsync(request);


            return addResult.ToActionResult();
        }
        
    }
}
