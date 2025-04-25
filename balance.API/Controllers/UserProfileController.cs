using balance.services.DTOs.UserProfile;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace balance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UserProfileController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _serviceManager.UserProfileService.GetUserProfileAsync(userId);
            
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProfileById(string id)
        {
            var profile = await _serviceManager.UserProfileService.GetUserProfileAsync(id);
            
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _serviceManager.UserProfileService.UpdateUserProfileAsync(userId, updateProfileDto);
            
            if (profile == null)
                return BadRequest("Failed to update profile");

            return Ok(profile);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _serviceManager.UserProfileService.ChangePasswordAsync(userId, changePasswordDto);
            
            if (!result)
                return BadRequest("Failed to change password. Please ensure your current password is correct.");

            return Ok("Password changed successfully");
        }

        [HttpPut("profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] string pictureUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _serviceManager.UserProfileService.UpdateProfilePictureAsync(userId, pictureUrl);
            
            if (result == null)
                return BadRequest("Failed to update profile picture");

            return Ok(result);
        }

        [HttpPut("deactivate")]
        public async Task<IActionResult> DeactivateAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _serviceManager.UserProfileService.DeactivateUserAsync(userId);
            
            if (!result)
                return BadRequest("Failed to deactivate account");

            return Ok("Account deactivated successfully");
        }

        [HttpGet("users-by-role/{role}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            var users = await _serviceManager.UserProfileService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        [HttpGet("agents")]
        public async Task<IActionResult> GetAgents()
        {
            var agents = await _serviceManager.UserProfileService.GetUsersByRoleAsync("Agent");
            return Ok(agents);
        }
    }
}
