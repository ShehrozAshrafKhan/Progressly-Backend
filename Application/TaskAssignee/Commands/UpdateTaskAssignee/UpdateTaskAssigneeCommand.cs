using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAssignee.Commands.UpdateTaskAssignee
{
    public class UpdateTaskAssigneeCommand : IRequest<Response<bool>>
    {
        public Guid? TaskAssigneeId { get; set; }
        public Guid? TaskId { get; set; }
        public List<string>? AssignedBy { get; set; } 
        public DateTime? AssignedAt { get; set; }
    }
}
