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
    public class ProjectAssignee:BaseAuditableEntity
    {
        [Key]
        public Guid ProjectAssigneeId { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
