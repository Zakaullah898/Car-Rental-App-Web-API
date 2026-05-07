
using CarRentalApp.models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace CarRentalApp.Service
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(200).Height(200).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.SecureUrl.ToString();
            }
            else
            {
                throw new ArgumentException("File is empty.", nameof(file));
            }

        }
    }
}
