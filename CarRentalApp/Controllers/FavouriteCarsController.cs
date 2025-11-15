using CarRentalApp.CustomException;
using CarRentalApp.models;
using CarRentalApp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarRentalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteCarsController : ControllerBase
    {
        private ApiResponse _response;
        private readonly IFavouriteCarsService _favouriteCarsService;
        public FavouriteCarsController(IFavouriteCarsService favouriteCarsService)
        {
            _favouriteCarsService = favouriteCarsService;
            _response = new ApiResponse();
        }
        // POST: api/FavouriteCars/add-to-favourites 
        [HttpPost("AddToFavourite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> AddToFavourite([FromBody] FavouriteDTO model)
        {

            try
            {

                FavouriteDTO result = await _favouriteCarsService.AddToFavouritesAsync(model);

                _response.Message = "Car added to favourites successfully";
                _response.Data = result;
                _response.StatusCode = HttpStatusCode.Created;
                _response.status = true;
                //return CreatedAtRoute("GetFavouriteCarsByUserId", new { userId = model.UserId }, _response);
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

        // GET: api/FavouriteCars/get-favourites/{userId}
        [HttpGet("GetFavourites/{userId}", Name = "GetFavouriteCarsByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> GetFavouriteCarsByUserId(string userId)
        {
            try
            {
                var favouriteCars = await _favouriteCarsService.GetFavouriteCarsByUserIdAsync(userId);
                if(favouriteCars == null || !favouriteCars.Any())
                {
                    _response.Message = "No favourite cars found for the user.";
                }
                else { 
                    _response.Message = $"{favouriteCars.Count} favourite cars retrieved successfully";
                }
                _response.Data = favouriteCars;
                _response.StatusCode = HttpStatusCode.OK;
                _response.status = true;
                return Ok(_response);
            }
            catch (ArgumentNullException ex)
            {
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.status = false;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Message = "An error occurred while retrieving favourite cars.";
                _response.Errors!.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.status = false;
                return Ok(_response);
            }
        }
    }
}
