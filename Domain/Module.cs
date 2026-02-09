using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Module:BaseAuditableEntity
    {
        [Key]
        public Guid ModuleId { get; set; }
        public string? ModuleName { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }
        public ICollection<Task>? Tasks { get; set; }
    }
}
