using CarRentalApp.CustomException;
using CarRentalApp.Data;
using CarRentalApp.models;
using CarRentalApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Net;

namespace CarRentalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private ApiResponse _response;
        private readonly IAuthUserService _authService;
        private readonly IUserProfileService _userProfileService;
        public AuthController( IAuthUserService authUser, IUserProfileService userProfileService)
        {
            _response = new ApiResponse();
            _authService = authUser;
            _userProfileService = userProfileService;
        }

        [HttpPost("RegiterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> RegisterUser([FromBody] UserDTO dto)
        {
            try
            {

                var result = await _authService.RegiterUserAsync(dto);
                if (!result.Succeeded)
                {

                    _response.status = false;
                    _response.Errors = result.Errors.Select(e => e.Description).ToList();
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                await _authService.SendOtpAsync(dto.Email);
                _response.Data = new
                {
                    dto.UserName,
                    dto.Email,
                    message = "User registered successfully. OTP has been sent to your email."
                };

                _response.status = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (ConflictException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.Conflict;
                _response.status = false;
                return Ok(_response);
            }
            catch (BadRequestException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.status = false;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.status = false;

                return Ok(_response);
            }
        }

        [HttpPost("SignIN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginModel model)
        {
            try
            {

                
                var result = await _authService.Login(model);
                if (!result.IsSuccess)
                {
                    _response.status = false;
                    _response.Errors!.Add(result.Message!);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;

                }
                else
                {
                    var tokenData = _authService.GenerateJwtToken(model.Email);
                    if (!result.HasProfile)
                    {
                        _response.Data = new
                        {
                            UserData = "",
                            UserId = result.UserId,
                            tokenData = tokenData,
                            hasProfile = false,
                        };
                        _response.Message = result.Message;
                        _response.status = true;
                        _response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        var userProfile = await _userProfileService.GetUserProfileByUserIdAsync(result.UserId!);
                        _response.Data = new
                        {
                            UserData = userProfile,
                            UserId = result.UserId,
                            tokenData = tokenData,
                            hasProfile = true,
                        };
                        _response.Message = result.Message;
                        _response.status = true;
                        _response.StatusCode = HttpStatusCode.OK;
                    }

                    return Ok(_response);

                }

            }

            catch (KeyNotFoundException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.status = false;
                return Ok(_response);
            }
            catch (BadRequestException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.status = false;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.status = false;
                return Ok(_response);
            }
        }

        // For Otp varification 
        [HttpPost("OTPVarification")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse>> OTPVarification([FromBody] OtpVerifyRequest model)
        {
            try
            {

                bool result = await _authService.VerifyOtp(model);
                if (!result)
                {
                    _response.status = false;
                    _response.Errors!.Add("Invalid credentials. Please check your email and password.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;

                }
                else
                {

                    _response.Data = new
                    {
                        message ="You otp is valid"
                    };
                    _response.status = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);

                }

            }

            catch (KeyNotFoundException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.status = false;
                return Ok(_response);
            }
            catch (BadRequestException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.status = false;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.status = false;
                return Ok(_response);
            }
        }

        // for resend Otp 
        [HttpPost("resend-otp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> ResendOtp([FromBody] OtpResendRequest model)
        {
            try
            {
                bool result = await _authService.SendOtpAsync(model.Email);
                if (!result)
                {
                    _response.status = false;
                    _response.Errors!.Add("Failed to send OTP. Please try again.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _response.status = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = new { message = "OTP has been resent successfully." };
                return Ok(_response);
            }
            catch (ArgumentNullException ex)
            {
                _response.status = false;
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.status = false;
                _response.Errors!.Add("Internal server error: " + ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        // Api for forget password 
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            try
            {
                bool result = await _authService.SendOtpAsync(model.Email);
                if (!result)
                {
                    _response.status = false;
                    _response.Errors!.Add("Failed to send OTP. Please try again.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _response.status = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = new { message = "OTP has been sent to your email." };
                return Ok(_response);
            }
            catch (ArgumentNullException ex)
            {
                _response.status = false;
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.status = false;
                _response.Errors!.Add("Internal server error: " + ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }


        // Api for reset password
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            try
            {
                bool result = await _authService.ResetPasswordAsync(model);
                _response.status = result;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = new { message = "Password has been reset successfully." };
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.status = false;
                return Ok(_response);
            }
            catch (BadRequestException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.status = false;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.status = false;
                return Ok(_response);
            }
        }

       
    }
}
