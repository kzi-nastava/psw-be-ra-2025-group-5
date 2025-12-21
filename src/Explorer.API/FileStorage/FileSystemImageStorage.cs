using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.Stakeholders.API.Public;

namespace Explorer.API.FileStorage
{
    public class FileSystemImageStorage : IImageStorage
    {
        private readonly IWebHostEnvironment _env;

        public FileSystemImageStorage(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string SaveImage(
            string entityType,
            long entityId,
            byte[] data,
            string contentType)
        {
            var ext = contentType switch
            {
                "image/png" => ".png",
                "image/jpeg" => ".jpg",
                "image/jpg" => ".jpg",
                _ => throw new ArgumentException("Unsupported image type")
            };

            var folder = Path.Combine(
                _env.WebRootPath,
                "images",
                entityType.ToLower(),
                entityId.ToString()
            );

            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            File.WriteAllBytes(fullPath, data);

            return $"/images/{entityType.ToLower()}/{entityId}/{fileName}";
        }

        public void Delete(string relativePath)
        {
            var fullPath = Path.Combine(
                _env.WebRootPath,
                relativePath.TrimStart('/')
            );

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
