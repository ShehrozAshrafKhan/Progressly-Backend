using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.DTOs
{
    public class TaskCodeChangesDTO
    {
        public Guid TaskCodeChangeId { get; set; }
        public Guid TaskId { get; set; }
        public Guid TagId { get; set; }
        public string? EntryType { get; set; }

        public string? OldFileName { get; set; }
        public string? OldExtension { get; set; }
        public string? OldFilePath { get; set; }

        public string? NewFileName { get; set; }
        public string? NewExtension { get; set; }
        public string? NewFilePath { get; set; }

    }
}
