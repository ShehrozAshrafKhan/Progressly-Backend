using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskTags.DTOs
{
    public class TaskTagDTO
    {
        public Guid? TaskId { get; set; }
        public Guid? TagId { get; set; }
        public string? TagName { get; set; }
    }
}
