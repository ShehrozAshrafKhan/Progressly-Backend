using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAttachments.DTOs
{
    public class TaskAttachmentDTO
    {
        public Guid TaskAttachmentId { get; set; }
        public string FileName { get; set; } = null!;
        public Guid? TaskId { get; set; }
    }
}
