namespace CarRentalApp.Data
{
    public class FavoriteCars
    {
       
        public int FavoriteId { get; set; }

        // Foreign Key to User
        public string? UserId { get; set; }
        public int CarId { get; set; }

        // Date when user added this car to favorites
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public virtual User? User { get; set; }
        public virtual Car? Car { get; set; }

        //internal bool Any()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
