using Explorer.Blog.API.Public;

namespace Explorer.API.FileStorage
{
    public class FileSystemImageStorage : IImageStorage
    {
        private readonly IWebHostEnvironment _env;
        public FileSystemImageStorage(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string SaveBlogImage(long blogId, byte[] data, string contentType)
        {
            var ext = contentType switch
            {
                "image/png" => ".png",
                "image/jpeg" => ".jpg",
                _ => ".bin"
            };

            var folder = Path.Combine(
                _env.WebRootPath,
                "images",
                "blog",
                blogId.ToString()
            );

            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            File.WriteAllBytes(fullPath, data);

            return $"/images/blog/{blogId}/{fileName}";
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
