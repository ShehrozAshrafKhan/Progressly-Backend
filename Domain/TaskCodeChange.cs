using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TaskCodeChange:BaseAuditableEntity
    {
        [Key]
        public Guid TaskCodeChangeId { get; set; }
        public Guid TaskId { get; set; }
        public Task? Task { get; set; }
        public Guid TagId { get; set; }
        public Tag? Tag { get; set; }
        public string? EntryType { get; set; } // "file" or "text"

        public string? OldFileName { get; set; }
        public string? OldExtension { get; set; }
        public string? OldFilePath { get; set; }

        public string? NewFileName { get; set; }
        public string? NewExtension { get; set; }
        public string? NewFilePath { get; set; }

    }
}
