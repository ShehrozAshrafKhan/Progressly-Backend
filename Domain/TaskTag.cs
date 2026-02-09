using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TaskTag:BaseAuditableEntity
    {
        [Key]
        public Guid TaskTagId { get; set; }
        public Guid? TaskId { get; set; }
        public Task? Task { get; set; }
        public Guid? TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
