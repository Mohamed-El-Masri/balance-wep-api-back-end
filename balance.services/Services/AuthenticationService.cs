using balance.domain;
using balance.services.DTOs.Authentication;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace balance.services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<Applicationuser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthenticationService(
            UserManager<Applicationuser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterDto registerDto)
        {
            // Check if user already exists
            var userByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            var userByUsername = await _userManager.FindByNameAsync(registerDto.UserName);

            if (userByEmail != null || userByUsername != null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User already exists"
                };
            }

            // Create new user
            var user = new Applicationuser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = $"User registration failed: {errors}"
                };
            }

            // Ensure roles exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                
            if (!await _roleManager.RoleExistsAsync("Agent"))
                await _roleManager.CreateAsync(new IdentityRole("Agent"));
                
            if (!await _roleManager.RoleExistsAsync("Customer"))
                await _roleManager.CreateAsync(new IdentityRole("Customer"));

            // Assign role
            await _userManager.AddToRoleAsync(user, registerDto.Role);

            // Generate token
            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.GenerateTokenAsync(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Return response
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User registered successfully",
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.Now.AddMinutes(60),
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Find user by email or username
            var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUsername)
                ?? await _userManager.FindByNameAsync(loginDto.EmailOrUsername);

            // Check if user exists
            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email/username or password"
                };
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Account is deactivated. Please contact support."
                };
            }

            // Validate password
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid email/username or password"
                };
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Generate token
            var token = await _tokenService.GenerateTokenAsync(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Return response
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful",
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.Now.AddMinutes(60),
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                return result.Succeeded;
            }

            return true;
        }

        public async Task<List<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                return result.Succeeded;
            }
            return true;
        }
    }
}
