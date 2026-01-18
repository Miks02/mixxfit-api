using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitalOps.API.DTO.User;
using VitalOps.API.Services.Interfaces;

namespace VitalOps.API.Controllers
{
    [Authorize]
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<ActionResult<ProfilePageDto>> GetMyProfile(CancellationToken cancellationToken = default)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            return await _profileService.GetUserProfileAsync(userId!, cancellationToken);
        }
    }
}
