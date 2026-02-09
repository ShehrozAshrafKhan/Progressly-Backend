using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Tag: BaseAuditableEntity
    {
        [Key]
        public Guid TagId { get; set; }
        public string? TagName { get; set; }
        public bool? IsActive { get; set; }
        public ICollection<TaskTag>? TaskTags { get; set; }

    }
}
