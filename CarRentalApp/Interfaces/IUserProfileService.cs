using CarRentalApp.Data;
using CarRentalApp.DTOs;

namespace CarRentalApp.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileDTO> CreateUserProfileAsync(UserProfileCreateDTO Entity);
        Task<UserProfileDTO> UpdateUserProfileAsync(string userId, UserProfileCreateDTO Entity);
        Task<UserProfileDTO> GetUserProfileByUserIdAsync(string userId);
    }
}
