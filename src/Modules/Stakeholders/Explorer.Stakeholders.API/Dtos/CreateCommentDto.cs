using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class CreateCommentDto
    {
        [Required]
        [StringLength(2000)]
        public string Content { get; set; }
    }
}
