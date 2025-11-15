using System.Net;

namespace CarRentalApp.models
{
    public class ApiResponse
    {
        public string? Message { get; set; }
        public bool status { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public dynamic? Data { get; set; }

        public List<string>? Errors { get; set; }

        public ApiResponse()
        {
            Errors = new List<string>();  // Always initialized
        }
    }
}
