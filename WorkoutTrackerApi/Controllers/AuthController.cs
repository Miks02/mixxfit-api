using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Tasks;
using WorkoutTrackerApi.DTO.Auth;
using WorkoutTrackerApi.Services.Interfaces;

namespace WorkoutTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            List<string> errors = new List<string>();
            
            if (!ModelState.IsValid)
            {
                var errorList = new List<string>();
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];

                    if (state == null) continue;

                    if (state.Errors.Any())
                    {
                        foreach (var error in state.Errors)
                        {
                            errorList.Add(error.ErrorMessage);
                        }
                    }
                }

                return BadRequest(new { message = "One or more validation errors occurred", errors = errorList });
            }

            var result = await _authService.RegisterAsync(request);

            if (!result.IsSucceeded)
            {
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }

                return BadRequest(new { errors = errors });
            }

            return Ok(new { message = "Registration completed", data = result.Payload });

        }
        
    }
}
