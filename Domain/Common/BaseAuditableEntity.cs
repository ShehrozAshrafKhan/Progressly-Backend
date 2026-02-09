using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class BaseAuditableEntity
    {
        public string? EnterById { get; set; }
        public string? EnterBy { get; set; }
        public DateTime Created { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

}
