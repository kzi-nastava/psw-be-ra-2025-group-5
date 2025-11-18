using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class BlogImageDto
    {
        public long Id { get; set; }
        public int Order { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
