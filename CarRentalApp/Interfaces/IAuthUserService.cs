using CarRentalApp.DTOs;
using CarRentalApp.models;
using Microsoft.AspNetCore.Identity;
namespace CarRentalApp.Interfaces
{
    public interface IAuthUserService
    {
        Task<IdentityResult> RegiterUserAsync(UserDTO dto);
        Task<LoginResponse> Login(LoginModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest model);
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task<bool> SendOtpAsync(string email);
        Task<bool> VerifyOtp(OtpVerifyRequest model);
        //dynamic? GenerateJwtToken(string user);
        Task<JwtTokenResponseDto> GenerateJwtToken(string userName, string userId);
        Task<JwtTokenResponseDto> RefreshAccessTokenAsync(string refreshToken);
    }
}
