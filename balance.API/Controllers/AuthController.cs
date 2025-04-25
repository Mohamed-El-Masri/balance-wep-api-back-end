using balance.services.DTOs.Authentication;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace balance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceManager.AuthenticationService.RegisterUserAsync(registerDto);
            
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
            
            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("create-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var result = await _serviceManager.AuthenticationService.CreateRoleAsync(roleName);
            
            if (!result)
                return BadRequest("Failed to create role");

            return Ok($"Role {roleName} created successfully");
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var result = await _serviceManager.AuthenticationService.AssignRoleAsync(userId, roleName);
            
            if (!result)
                return BadRequest("Failed to assign role");

            return Ok($"Role {roleName} assigned successfully to user");
        }
        
        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _serviceManager.AuthenticationService.GetRolesAsync(userId);
            
            if (roles.Count == 0)
                return NotFound("User not found or has no roles");
                
            return Ok(roles);
        }
    }
}
