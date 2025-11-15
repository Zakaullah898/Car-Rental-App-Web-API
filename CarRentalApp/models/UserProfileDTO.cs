namespace CarRentalApp.models
{
    public class UserProfileDTO
    {
        public int ProfileId { get; set; }
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? PostCode { get; set; }
        public string? Address { get; set; }
        public string? DrivingLicenseNo { get; set; }

        public string? ProfileImageUrl { get; set; }
        public string? LicenseFrontImage { get; set; }
        public string? LicenseBackImage { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
