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
    public class TaskAssignee:BaseAuditableEntity
    {
        [Key]
        public Guid? TaskAssigneeId { get; set; }
        public Guid? TaskId { get; set; }
        public Task? Task { get; set; }
        public string? AssignedBy { get; set; }
        public ApplicationUser? AssignedByUser { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
