using balance.services.DTOs.UserProfile;

namespace balance.services.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(string userId);
        Task<UserProfileDto> UpdateUserProfileAsync(string userId, UpdateProfileDto updateProfileDto);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<string> UpdateProfilePictureAsync(string userId, string pictureUrl);
        Task<bool> DeactivateUserAsync(string userId);
        Task<IEnumerable<UserProfileDto>> GetUsersByRoleAsync(string role);
    }
}
