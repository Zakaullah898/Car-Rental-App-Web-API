using CarRentalApp.models;
using CarRentalApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarRentalApp.Controllers
{
    [Authorize(AuthenticationSchemes = "AuthForLocal", Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private ApiResponse _response;
        private readonly ICarsService _carsService;
        public CarsController(ICarsService carsService)
        {
            _carsService = carsService;
            _response = new ApiResponse();
        }
        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ApiResponse>>> GetAllCars()
        {

            try
            {
                _response.Data = await _carsService.GetAllCarsAsync();
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
