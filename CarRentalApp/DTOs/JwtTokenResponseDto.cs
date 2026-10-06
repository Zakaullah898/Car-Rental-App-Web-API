namespace CarRentalApp.DTOs
{
    public class JwtTokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime AccessTokenExpiry { get; set; }
    }
}
