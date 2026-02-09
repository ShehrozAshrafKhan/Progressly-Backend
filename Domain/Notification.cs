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
    public class Notification:BaseAuditableEntity
    {
        [Key]
        public Guid NotificationId { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime NotificationDate { get; set; } = DateTime.UtcNow;
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public Guid? TaskId { get; set; }
        public Task? Task { get; set; }
    }
}
