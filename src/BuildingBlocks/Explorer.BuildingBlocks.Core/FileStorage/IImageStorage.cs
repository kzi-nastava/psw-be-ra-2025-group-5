using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Core.FileStorage
{
    public interface IImageStorage
    {
        string SaveImage(string entityType, long entityId, byte[] data, string contentType);
        void Delete(string relativePath);
    }
}
