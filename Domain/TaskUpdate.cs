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
    public class TaskUpdate:BaseAuditableEntity
    {
        [Key]
        public Guid TaskUpdateId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; }
        public int ProgressPercent { get; set; } // 0 - 100
        public string? UpdateNotes { get; set; }

        public Guid? TaskId { get; set; }
        public Task? Task { get; set; }

        public string? UpdatedBy { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
