using CarRentalApp.Data;
using CarRentalApp.models;

namespace CarRentalApp.Service
{
    public interface IUserProfileService
    {
        Task<UserProfileDTO> CreateUserProfileAsync(UserProfileCreateDTO Entity);
        Task<UserProfileDTO> UpdateUserProfileAsync(string userId, UserProfileCreateDTO Entity);
        Task<UserProfileDTO> GetUserProfileByUserIdAsync(string userId);
    }
}
