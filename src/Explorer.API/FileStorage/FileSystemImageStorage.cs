using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.Stakeholders.API.Public;

namespace Explorer.API.FileStorage
{
    public class FileSystemImageStorage : IImageStorage
    {
        private readonly string _basePath;

        public FileSystemImageStorage()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "UserUploads");
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
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

            var folder = Path.Combine(_basePath, entityType.ToLower(), entityId.ToString());
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            File.WriteAllBytes(fullPath, data);

            return Path.Combine(entityType.ToLower(), entityId.ToString(), fileName).Replace("\\", "/");
        }

        public void Delete(string relativePath)
        {
            var fullPath = Path.Combine(_basePath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
