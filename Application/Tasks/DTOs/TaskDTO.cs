using Application.TaskAssignee.DTOs;
using Application.TaskAttachments.DTOs;
using Application.TaskTags.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.DTOs
{
    public class TaskDTO
    {
        public Guid TaskId { get; set; }
        public string? Title { get; set; }
        public string? TaskNo { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public double? EstimatedHours { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCompletedRequest { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public List<TaskAttachmentDTO>? Attachments { get; set; }
        public List<TaskAssigneeDTO>? Assignees { get; set; }
        public List<TaskTagDTO>? TaskTags { get; set; }
    }
}
