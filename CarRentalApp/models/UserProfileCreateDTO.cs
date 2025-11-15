namespace CarRentalApp.models
{
    public class UserProfileCreateDTO
    {
        public string? UserId { get; set; } // foregn key to User table
        public int ProfileId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? PostCode { get; set; }
        public string? Address { get; set; }
        public string? DrivingLicenseNo { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public IFormFile? LicenseFrontImage { get; set; }
        public IFormFile? LicenseBackImage { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
