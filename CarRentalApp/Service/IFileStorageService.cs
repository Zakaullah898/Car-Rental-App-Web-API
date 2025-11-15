namespace CarRentalApp.Service
{
    public interface IFileStorageService
    {
        Task<string?> SaveFileAsync(IFormFile? file, string folder = "uploads");
    }
}
