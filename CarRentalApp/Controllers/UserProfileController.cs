using CarRentalApp.CustomException;
using CarRentalApp.Data;
using CarRentalApp.models;
using CarRentalApp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System.Net;

namespace CarRentalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private ApiResponse _response;
        private readonly IUserProfileService _userProfileService;
        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
            _response = new ApiResponse();
        }
        // Endpoint to create user profile
        [HttpPost("uers_profile_creation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> UserProfileCreation([FromForm] UserProfileCreateDTO model)
        {
            Console.WriteLine("User profile creation endpoint hit");
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"❌ Field: {state.Key} — Error: {error.ErrorMessage}");
                    }
                }

                return BadRequest(ModelState);
            }
            try
            {

                UserProfileDTO userProfile = await _userProfileService.CreateUserProfileAsync(model);
                _response.Message = "User profile Created successfully";
                _response.Data = userProfile;
                //{
                //    FullName = userProfile.FullName,
                //    Email = userProfile.Email,
                //    Phone = userProfile.Phone,
                //    Gender = userProfile.Gender,
                //    PostCode = userProfile.PostCode,
                //    Address = userProfile.Address,

                //    CreatedAt = userProfile.CreatedAt
                //};
                _response.StatusCode = HttpStatusCode.Created;
                _response.status = true;
                model.UserId = userProfile.UserId!;
                return CreatedAtRoute("GetUserProfileById", new { userId = model.UserId }, _response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.status = false;
                return Ok(_response);
            }
            catch (ArgumentNullException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.status = false;
                return Ok(_response);
            }
            catch (ConflictException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.Conflict;
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

        // Endpoint to get user profile by profile id
        [HttpGet("{userId}", Name ="GetUserProfileById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetUserProfileById(string userId)
        {
            try
            {
                UserProfileDTO userProfile = await _userProfileService.GetUserProfileByUserIdAsync(userId);
                _response.Message = "User profile fetched successfully";
                _response.Data = userProfile;
                _response.StatusCode = HttpStatusCode.OK;
                _response.status = true;
                return Ok(_response);
            }
            catch (BadRequestException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.status = false;
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
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

        // Endpoint to update user profile
        [HttpPut("UpdateUserProfile/{userId}", Name = "UpdateUserProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> UpdateUserProfile(string userId, [FromForm] UserProfileCreateDTO model)
        {
           
            try
            {
                UserProfileDTO UpdateduserProfile = await _userProfileService.UpdateUserProfileAsync(userId, model);
                _response.Message = "User profile updated successfully";
                _response.Data = UpdateduserProfile;
                _response.StatusCode = HttpStatusCode.Created;
                _response.status = true;
                model.UserId = UpdateduserProfile.UserId!;
                return CreatedAtRoute("GetUserProfileById", new { userId = model.UserId }, _response);
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
