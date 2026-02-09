using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TaskAttachment:BaseAuditableEntity
    {
        [Key]
        public Guid TaskAttachmentId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public Guid? TaskId { get; set; }
        public Task? Task { get; set; }

    }
}
