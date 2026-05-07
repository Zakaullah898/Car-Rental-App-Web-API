using CarRentalApp.CustomException;
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
    public class BookingController : ControllerBase
    {
        private ApiResponse _response;
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
            _response = new ApiResponse();
        }

        // POST: api/Booking/CreateBooking
        [HttpPost("CreateBooking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> AddToFavourite([FromBody] CreateBookingRequest model)
        {

            try
            {

                BookingResponse result = await _bookingService.CreateBookingAsync(model);

                _response.Message = "Booking and Payment created successfully!";
                _response.Data = result;
                _response.StatusCode = HttpStatusCode.Created;
                _response.status = true;
                //return CreatedAtRoute("GetFavouriteCarsByUserId", new { userId = model.UserId }, _response);
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
