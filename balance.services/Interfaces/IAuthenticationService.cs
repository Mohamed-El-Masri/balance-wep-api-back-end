using balance.services.DTOs.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace balance.services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> AssignRoleAsync(string userId, string role);
        Task<List<string>> GetRolesAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<List<string>> GetAllRoles();
        Task<List<UserDto>> GetAllUsers();
        Task<AuthResponseDto> RemoveRoleFromUser(string userId,string roleName);
        Task<AuthResponseDto> RemoveUser(string userId);
        Task<AuthResponseDto> LogOut();




        //  Task<bool> CreateRoleAsync(string roleName);
    }
}
