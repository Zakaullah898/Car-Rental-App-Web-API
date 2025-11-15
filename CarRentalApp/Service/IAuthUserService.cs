using CarRentalApp.models;
using Microsoft.AspNetCore.Identity;
namespace CarRentalApp.Service
{
    public interface IAuthUserService
    {
        Task<IdentityResult> RegiterUserAsync(UserDTO dto);
        Task<LoginResponse> Login(LoginModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest model);
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task<bool> SendOtpAsync(string email);
        Task<bool> VerifyOtp(OtpVerifyRequest model);
        dynamic? GenerateJwtToken(string user);
    }
}
