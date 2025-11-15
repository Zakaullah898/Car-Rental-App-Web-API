using AutoMapper;
using CarRentalApp.CustomException;
using CarRentalApp.Data;
using CarRentalApp.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using SendGrid.Helpers.Errors.Model;
using SendGrid.Helpers.Mail;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CarRentalApp.Service
{
    public class AuthUserService : IAuthUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly CarRentalDBContext _dbContext;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;
        public AuthUserService(UserManager<User> userManager,
            SignInManager<User> signInManager, 
            IConfiguration configuration, 
            CarRentalDBContext dbContext,
            IMapper mapper)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;

            _jwtIssuer = _configuration.GetValue<string>("LocalIssuer")!;
            _jwtAudience = _configuration.GetValue<string>("LocalAudience")!;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        // Generating JWT token
        public dynamic? GenerateJwtToken(string user)
        {
            var Key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("LocalScretKey")!);
            var TokenHandler = new JwtSecurityTokenHandler();
            var ExpiresIn = DateTime.UtcNow.AddHours(1);

            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user),
                    new Claim(ClaimTypes.Role,"user")
                }),
                Expires = ExpiresIn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha512)
            };
            var token = TokenHandler.CreateToken(TokenDescriptor);
            return new
            {
                token = TokenHandler.WriteToken(token),
                tokenType = "Bearer",
                expiresIn = (int)(ExpiresIn - DateTime.UtcNow).TotalMinutes  // ✅ real seconds
            };


        }

        // for Login
        public async Task<LoginResponse> Login(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                throw new BadRequestException("Username or Email or passwor can not empty or null");
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            //user.Id
            if (user == null)
            {

                throw new KeyNotFoundException($"User not found with email {model.Email}.");
            }

            if (!user.EmailConfirmed)
            {
                throw new BadRequestException("Please verify your account through OTP before logging in.");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return new LoginResponse
                {
                    UserId = user.Id,
                    IsSuccess = false,
                    HasProfile = false,
                    Message = "Invalid login attempt."
                };
            }
 
            return new LoginResponse
            {
                UserId = user.Id,
                IsSuccess = true,
                HasProfile = user.HasProfile,
                Message = user.HasProfile ? "Login successful" : "Profile creation required"
            };


        }

        // for user registration
        public async Task<IdentityResult> RegiterUserAsync(UserDTO dto)
        {
            if (string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                throw new BadRequestException("Username or Email or passwor can not empty or null");
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new ConflictException($"A user with the email '{dto.Email}' already exists.");
            }
   
            var NewUser = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                HasProfile = false
            };
            var resutl = await _userManager.CreateAsync(NewUser, dto.Password);
            return resutl;
        }

       // Sendign email using sendgrid for otp generation
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_configuration["SendGrid:FromEmail"], _configuration["SendGrid:FromName"]);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            await client.SendEmailAsync(msg);
        }


        // Sending OTP for varification
        public async Task<bool> SendOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                throw new ArgumentNullException($"User not found with email {email}.");
            }
            // Generate 5-digit OTP
            var otpCode = new Random().Next(10000, 99999).ToString();
            var otpEntry = new OTPVerification
            {
                UserId = user.Id,
                OtpCode = otpCode,
                GeneratedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsVerified = false
            };
            await _dbContext.OTPVerifications.AddAsync(otpEntry);
            await _dbContext.SaveChangesAsync();

            // Send email
            string subject = "Your OTP Code - Car Rental App";
            string message = $"Your verification code is: <b>{otpCode}</b><br/>It will expire in 5 minutes.";
            await SendEmailAsync(email, subject, message);
            return true;
        }
        // Varifying the OTP
        public async Task<bool> VerifyOtp(OtpVerifyRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var otpRecord = await _dbContext.OTPVerifications
                .Where(o => o.UserId == user.Id && o.OtpCode == model.OtpCode)
                .OrderByDescending(o => o.GeneratedAt)
                .FirstOrDefaultAsync();

            if (otpRecord == null)
            {
                throw new BadRequestException("Invalid OTP");
            }


            if (otpRecord.ExpiresAt < DateTime.UtcNow)
            {
                throw new BadRequestException("OTP expired");
            };

            otpRecord.IsVerified = true;
            await _dbContext.SaveChangesAsync();

            // Optionally mark user email as confirmed
            
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
         


            return true;
        }

        // Resetting password
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            // Step 1: Check if the latest OTP for password reset was verified
            var otpVerified = await _dbContext.OTPVerifications
                .Where(o => o.UserId == user.Id && o.IsVerified && o.OtpCode == model.OtpCode)
                .OrderByDescending(o => o.GeneratedAt)
                .FirstOrDefaultAsync();
            Console.WriteLine($"otp : {otpVerified}");

            if (otpVerified == null)
            {
                throw new BadRequestException("Please verify your OTP before resetting the password.");
            }

            // Step 3: Generate Identity reset token and reset password
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!resetResult.Succeeded)
            {
                var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                throw new BadRequestException($"Password reset failed: {errors}");
            }

            

            return true;
        }
    }
}
