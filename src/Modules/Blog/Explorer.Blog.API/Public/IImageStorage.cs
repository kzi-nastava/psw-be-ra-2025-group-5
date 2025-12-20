using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Public
{
    public interface IImageStorage
    {
        string SaveBlogImage(long blogId, byte[] data, string contentType);
        void Delete(string relativePath);
    }
}
