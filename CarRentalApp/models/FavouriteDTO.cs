namespace CarRentalApp.models
{
    public class FavouriteDTO
    {
        public int FavoriteId { get; set; }

        // Foreign Key to User
        public string? UserId { get; set; }
        public int CarId { get; set; }

        // Date when user added this car to favorites
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
