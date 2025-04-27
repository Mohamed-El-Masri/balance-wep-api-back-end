using balance.domain;
using balance.services.DTOs.Authentication;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

           

            // Assign role is customer 
            await _userManager.AddToRoleAsync(user,"Customer");

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

        public async Task<List<string>> GetAllRoles()
        {
            var Roles= await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Roles;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); 

                var userDto = new UserDto
                {
                    UserId = user.Id,
                    CountOfRoles = roles.Count,    
                    Email = user.Email,
                    Username=user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()  

                };

                userDtos.Add(userDto);
            }

            return userDtos;
        }

        public async Task<AuthResponseDto> RemoveRoleFromUser(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User ID and Role Name cannot be null or empty"
                };
            }

            // البحث عن المستخدم
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            // التحقق من وجود الدور
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Role not found"
                };
            }

            // التحقق من أن المستخدم لديه هذا الدور
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (!isInRole)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User does not have this role"
                };
            }

            // محاولة إزالة الدور
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = $"Role '{roleName}' removed successfully from user"
                };
            }

            // في حالة وجود أخطاء
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        public async Task<AuthResponseDto> RemoveUser(string userId)
        {
            if (string.IsNullOrEmpty(userId) )
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User ID  cannot be null or empty"
                };
            }

            // البحث عن المستخدم
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }


            
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = $"user '{user.UserName}' removed successfully"
                };
            }

            // في حالة وجود أخطاء
            return new AuthResponseDto
            {
                IsSuccess = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
        public async Task<AuthResponseDto> LogOut()
        {
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User logged out successfully"
            };
        }

        //public async Task<bool> CreateRoleAsync(string roleName)
        //{
        //    if (!await _roleManager.RoleExistsAsync(roleName))
        //    {
        //        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        //        return result.Succeeded;
        //    }
        //    return true;
        //}
    }
}
