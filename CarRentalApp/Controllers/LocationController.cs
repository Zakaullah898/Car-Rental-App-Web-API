using CarRentalApp.models;
using CarRentalApp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarRentalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private ApiResponse _response;
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
            _response = new ApiResponse();
        }
        // api/location/get-all
        [HttpGet("get-all-cities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ApiResponse>>> GetAllCars()
        {

            try
            {
                _response.Data = await _locationService.GetAllLocationsAsync();
                _response.status = true;
                _response.StatusCode = HttpStatusCode.OK;
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
