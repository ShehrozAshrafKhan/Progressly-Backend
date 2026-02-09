using Domain.Common;
using Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ActivityLog:BaseAuditableEntity
    {
        [Key]
        public Guid ActivityLogId { get; set; }
        public DateTime ActivityDate { get; set; } = DateTime.UtcNow.Date;
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid? TaskId { get; set; }
        public Task? Task { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }  // e.g., "Create Task"
        public string? Entity { get; set; }  // e.g., "Task"
        public string? Details { get; set; } // JSON or description
    }
}
