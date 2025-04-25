using balance.services.DTOs.Authentication;

namespace balance.services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> AssignRoleAsync(string userId, string role);
        Task<List<string>> GetRolesAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> CreateRoleAsync(string roleName);
    }
}
