using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAssignee.DTOs
{
    public class TaskAssigneeDTO
    {
        public Guid? TaskAssigneeId { get; set; }
        public Guid? TaskId { get; set; }
        public string? TaskName { get; set; }
        public string? AssignedBy { get; set; }
        public string? AssignedUserName { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
