using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreAPI.Dto;
using OnlineStoreAPI.Services;
using YourProject.Models;

namespace OnlineStoreAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IAuthService _authService;

        public ProfileController(IAuthService authService)
        {
            _authService = authService;
        }

        // Get current user profile
        [HttpGet("me")]
        public async Task<ActionResult<ApplicationUser>> GetProfile()
        {
            var user = await _authService.GetCurrentUserAsync();
            if (user == null)
                return Unauthorized("User not authenticated");

            return Ok(user);
        }
    }

}


