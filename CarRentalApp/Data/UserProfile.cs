using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalApp.Data
{
    public class UserProfile
    {
        public int ProfileId { get; set; }
        public string? UserId { get; set; } // FK to User table
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; } // "Male" | "Female" | "Other"
        public string? PostCode { get; set; }
        public string? Address { get; set; }
        public string? ProfileImageUrl { get; set; } // Profile picture path
        public string? DrivingLicenseNo { get; set; }
        public string? LicenseFrontImage { get; set; }
        public string? LicenseBackImage { get; set; }

        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 

        // Navigation property
        public virtual  User? User { get; set; }
    }
}
