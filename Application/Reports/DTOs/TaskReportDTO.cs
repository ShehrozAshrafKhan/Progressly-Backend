using Application.TaskAssignee.DTOs;
using Application.TaskAttachments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.DTOs
{
    public class TaskReportDTO
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
        public DateTime? Created { get; set; }
        public Guid? ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public Guid TaskAttachmentId { get; set; }
        public string FileName { get; set; } = null!;
        public Guid? TaskAssigneeId { get; set; }
        public string? TaskName { get; set; }
        public string? AssignedBy { get; set; }
        public string? UserName { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
