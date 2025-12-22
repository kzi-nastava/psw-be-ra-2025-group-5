using Explorer.BuildingBlocks.Core.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Tests
{
    public class InMemoryImageStorage : IImageStorage
    {
        public Dictionary<string, byte[]> StoredImages { get; } = new();

        public string SaveImage(string type, long id, byte[] content, string contentType)
        {
            var path = $"/fake/{type}/{id}/{Guid.NewGuid()}.jpg";
            StoredImages[path] = content;
            return path;
        }

        public void Delete(string path)
        {
            StoredImages.Remove(path);
        }
    }
}
