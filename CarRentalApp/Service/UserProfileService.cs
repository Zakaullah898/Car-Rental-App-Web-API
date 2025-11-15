using AutoMapper;
using CarRentalApp.CustomException;
using CarRentalApp.Data;
using CarRentalApp.Data.Repository;
using CarRentalApp.models;
using Microsoft.AspNetCore.Identity;
using SendGrid.Helpers.Errors.Model;

namespace CarRentalApp.Service
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ICarRentalRepository<UserProfile> _userProfileRepository;
        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;    
        public UserProfileService(
            ICarRentalRepository<UserProfile> userProfileRepository,
            IMapper mapper,
            UserManager<User> userManager,
            IFileStorageService fileStorage
            )
        {
            _userProfileRepository = userProfileRepository;
            _userManager = userManager;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }
        public async Task<UserProfileDTO> CreateUserProfileAsync(UserProfileCreateDTO Entity)
        {
            Console.WriteLine("Creating user profile...");
            if (Entity == null)
            {
                throw new ArgumentNullException(nameof(Entity), "UserProfileDTO cannot be null");
            }
            var user = await _userManager.FindByEmailAsync(Entity.Email!);
            Console.WriteLine($"User Id {user.Id}");
            if (user == null)
            {
                throw new KeyNotFoundException($"User not found with email {Entity.Email}.");
            }
            Entity.UserId = user.Id;
            var ExistingProfile = await _userProfileRepository.GetAsync(up =>  up.UserId == Entity.UserId, true);
            if(ExistingProfile != null)
            {
                throw new ConflictException("User profile already exists for this user. Please use update method for updation");
            }
            // Save uploaded files via file storage service
            Console.WriteLine("Saving uploaded files...");
            var profileImageUrl = await _fileStorage.SaveFileAsync(Entity.ProfileImage);
            var licenseFrontImage = await _fileStorage.SaveFileAsync(Entity.LicenseFrontImage);
            var licenseBackImage = await _fileStorage.SaveFileAsync(Entity.LicenseBackImage);
            Console.WriteLine("Files saved successfully.");
            // Map DTO to entity
            var userProfile = _mapper.Map<UserProfile>(Entity);
            userProfile.ProfileImageUrl = profileImageUrl;
            userProfile.LicenseFrontImage = licenseFrontImage;
            userProfile.LicenseBackImage = licenseBackImage;
            userProfile.CreatedAt = DateTime.UtcNow;
            userProfile.UpdatedAt = DateTime.UtcNow;
            await _userProfileRepository.CreateAsync(userProfile);
            var ReturningUserProfile = _mapper.Map<UserProfileDTO>(userProfile);
            // ✅ Update HasProfile flag
            user.HasProfile = true;
            await _userManager.UpdateAsync(user);

            return ReturningUserProfile;
        }

        public async Task<UserProfileDTO> GetUserProfileByUserIdAsync(string userId)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException("User ID cannot be null or empty.");
            }

            // Fetch user profile by UserId (no tracking for read-only operations)
            var userProfile = await _userProfileRepository.GetAsync(up => up.UserId == userId, false);

            // Handle not found case
            if (userProfile == null)
            {
                throw new KeyNotFoundException($"User profile not found for User ID: {userId}.");
            }

            // Map entity to DTO before returning
            var userProfileDto = _mapper.Map<UserProfileDTO>(userProfile);

            return userProfileDto;
        }


        public async Task<UserProfileDTO> UpdateUserProfileAsync(string userId, UserProfileCreateDTO Entity)
        {
            if(string.IsNullOrEmpty(userId))
            {
                throw new BadRequestException("User ID cannot be null or empty.");
            }
            var existingProfileTask = await _userProfileRepository.GetAsync(up => up.UserId == userId, true);
            if(existingProfileTask == null)
            {
                throw new KeyNotFoundException($"User profile not found with ID {userId}.");
            }
            var profileImageUrl = existingProfileTask.ProfileImageUrl;
            var licenseFrontImage = existingProfileTask.LicenseFrontImage;
            var licenseBackImage = existingProfileTask.LicenseBackImage;

            if (Entity.ProfileImage != null)
                profileImageUrl = await _fileStorage.SaveFileAsync(Entity.ProfileImage);
            if (Entity.LicenseFrontImage != null)
                licenseFrontImage = await _fileStorage.SaveFileAsync(Entity.LicenseFrontImage);
            if (Entity.LicenseBackImage != null)
                licenseBackImage = await _fileStorage.SaveFileAsync(Entity.LicenseBackImage);

            // Map DTO to entity
            var userProfile = _mapper.Map<UserProfile>(Entity);
            userProfile.ProfileImageUrl = profileImageUrl;
            userProfile.LicenseFrontImage = licenseFrontImage;
            userProfile.LicenseBackImage = licenseBackImage;
            // Replace this line in UpdateUserProfileAsync:
            // userProfile.CreatedAt = null;

            // With this line:
            userProfile.CreatedAt = existingProfileTask.CreatedAt;
            userProfile.UpdatedAt = DateTime.UtcNow;

            await _userProfileRepository.UpdateAsync(userProfile);
            var updatedProfile = _mapper.Map<UserProfileDTO>(userProfile);
            return updatedProfile;
        }
    }
}
