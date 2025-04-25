using AutoMapper;
using balance.domain;
using balance.services.DTOs.UserProfile;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace balance.services.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<Applicationuser> _userManager;
        private readonly IMapper _mapper;

        public UserProfileService(
            UserManager<Applicationuser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var userProfileDto = new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Address = user.Address,
                Bio = user.Bio,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            };

            return userProfileDto;
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(string userId, UpdateProfileDto updateProfileDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            // Update user properties
            user.FirstName = updateProfileDto.FirstName;
            user.LastName = updateProfileDto.LastName;
            user.PhoneNumber = updateProfileDto.PhoneNumber;
            user.Address = updateProfileDto.Address;
            user.Bio = updateProfileDto.Bio;
            user.DateOfBirth = updateProfileDto.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return null;

            return await GetUserProfileAsync(userId);
        }

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ChangePasswordAsync(
                user, 
                changePasswordDto.CurrentPassword, 
                changePasswordDto.NewPassword);

            return result.Succeeded;
        }

        public async Task<string> UpdateProfilePictureAsync(string userId, string pictureUrl)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            user.ProfilePictureUrl = pictureUrl;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return null;

            return pictureUrl;
        }

        public async Task<bool> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<IEnumerable<UserProfileDto>> GetUsersByRoleAsync(string role)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            
            var userProfiles = new List<UserProfileDto>();
            
            foreach (var user in usersInRole)
            {
                var roles = await _userManager.GetRolesAsync(user);
                
                userProfiles.Add(new UserProfileDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    Address = user.Address,
                    Bio = user.Bio,
                    CreatedAt = user.CreatedAt,
                    IsActive = user.IsActive,
                    Roles = roles.ToList()
                });
            }
            
            return userProfiles;
        }
    }
}
