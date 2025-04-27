using balance.services.DTOs.Authentication;
using balance.services.Interfaces;
using balance.services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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

        //[HttpPost("create-role")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> CreateRole([FromBody] string roleName)
        //{
        //    var result = await _serviceManager.AuthenticationService.CreateRoleAsync(roleName);
            
        //    if (!result)
        //        return BadRequest("Failed to create role");

        //    return Ok($"Role {roleName} created successfully");
        //}

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var result = await _serviceManager.AuthenticationService.AssignRoleAsync(userId, roleName);
            
            if (!result)
                return BadRequest("Failed to assign role");

            return Ok($"Role {roleName} assigned successfully to user");
        }
        
        [HttpGet("GetUserRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _serviceManager.AuthenticationService.GetRolesAsync(userId);
            
            if (roles.Count == 0)
                return NotFound("User not found or has no roles");
                
            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _serviceManager.AuthenticationService.GetAllRoles();

            if (result.Count==0)
                return BadRequest("no roles found");

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _serviceManager.AuthenticationService.GetAllUsers();

            if (result.Count == 0)
                return BadRequest("no roles found");

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser([FromQuery] string userId, [FromQuery] string roleName)
        {
            // التحقق من أن roleName ليس "null" أو فارغًا
            if (string.IsNullOrEmpty(roleName) || roleName == "null")
            {
                return BadRequest("Role name cannot be null or empty");
            }

            // التحقق من أن userId ليس فارغًا
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty");
            }

            var result = await _serviceManager.AuthenticationService.RemoveRoleFromUser(userId, roleName);

            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUser([FromQuery] string userId)
        {
    

            // التحقق من أن userId ليس فارغًا
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty");
            }

            var result = await _serviceManager.AuthenticationService.RemoveUser(userId);

            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            var result = await _serviceManager.AuthenticationService.LogOut();

            if (result.IsSuccess)
            {

               
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
