namespace CarRentalApp.models
{
    public class LoginResponse
    {
        public string? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public bool HasProfile { get; set; }
        public string? Message { get; set; }
    }
}
