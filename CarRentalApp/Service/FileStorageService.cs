
using Microsoft.Extensions.Configuration;

namespace CarRentalApp.Service
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;
        public FileStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string?> SaveFileAsync(IFormFile? file, string folder = "uploads")
        {
            if (file == null || file.Length == 0)
                return null;

            // Ensure directory exists
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Create unique, safe file name
            var originalName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(originalName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file asynchronously
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Get base URL from appsettings.json if available
            var baseUrl = _configuration["AppSettings:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                // Fallback to relative URL if not configured
                return $"/{folder}/{uniqueFileName}";
            }

            // Return absolute URL
            return $"{baseUrl}/{folder}/{uniqueFileName}";
        }
    }
}
