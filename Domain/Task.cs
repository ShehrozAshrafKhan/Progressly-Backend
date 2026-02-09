using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Task:BaseAuditableEntity
    {
        [Key]
        public Guid TaskId { get; set; }
        public string? Title { get; set; } = null!;
        public string? TaskNo { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } = "PENDING"; // Pending, In Progress, Completed
        public string? Priority { get; set; } // Low, Medium, High
        public double? EstimatedHours { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCompletedRequest { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ModuleId { get; set; }
        public Module? Module { get; set; }

        public ICollection<TaskUpdate>? Updates { get; set; }
        public ICollection<TaskAttachment>? Attachments { get; set; }
        public ICollection<TaskAssignee>? Assignees { get; set; }
        public ICollection<ActivityLog>? ActivityLogs { get; set; }
        public ICollection<TaskTag>? TaskTags { get; set; }
    }
}
