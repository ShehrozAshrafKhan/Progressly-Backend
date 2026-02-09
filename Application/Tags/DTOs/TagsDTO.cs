using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.DTOs
{
    public class TagsDTO
    {
        public Guid TagId { get; set; }
        public string? TagNo { get; set; }
        public string? TagName { get; set; }
    }
}
