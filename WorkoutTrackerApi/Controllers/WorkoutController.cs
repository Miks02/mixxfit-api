using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerApi.DTO.Global;
using WorkoutTrackerApi.DTO.Workout;
using WorkoutTrackerApi.Services.Interfaces;
using WorkoutTrackerApi.Services.Results;

namespace WorkoutTrackerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        
        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetMyWorkouts([FromQuery] string sort, [FromQuery] string search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var queryParams = new QueryParams(page, pageSize, search, sort);

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("An error occurred. User ID is null");
            }
            
            var getWorkoutsResult = await _workoutService.GetUserWorkoutsPagedAsync(queryParams, userId);

            if (!getWorkoutsResult.IsSucceeded)
            {
                List<Error> errors = [];
                
                foreach (var error in getWorkoutsResult.Errors)
                {
                    errors.Add(error);
                }

                return BadRequest(new { message = "Failed to fetch data", errors });
            }

            return Ok(getWorkoutsResult.Payload);
        }

        [HttpGet("workout/{id:int}")]
        public async Task<IActionResult> GetWorkout([FromRoute] int id)
        {
            var workouts = await _workoutService.GetWorkoutByIdAsync(id);
            
            if (!workouts.IsSucceeded)
            {
                List<Error> errors = [];
                
                foreach (var error in workouts.Errors)
                {
                    errors.Add(error);
                }

                return BadRequest(new { message = "Failed to fetch data", errors });
            }

            return Ok(workouts.Payload);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteWorkout([FromRoute] int id)
        {
            var workouts = await _workoutService.DeleteWorkoutAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            if (!workouts.IsSucceeded)
            {
                List<Error> errors = [];
                
                foreach (var error in workouts.Errors)
                {
                    errors.Add(error);
                }

                return BadRequest(new { message = "Failed to fetch data", errors });
            }

            return NoContent();
            
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkout([FromBody] WorkoutCreateRequest request)
        {

            var addResult = await _workoutService.AddWorkoutAsync(request);
            
            if (!addResult.IsSucceeded)
            {
                List<Error> errors = [];
                
                foreach (var error in addResult.Errors)
                {
                    errors.Add(error);
                }

                return BadRequest(new { message = "Failed to fetch data", errors });
            }

            return Ok(addResult.Payload);

        }
        
    }
}
