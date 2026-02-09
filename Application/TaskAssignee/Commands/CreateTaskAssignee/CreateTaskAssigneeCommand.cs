using Application.Common.Models;
using Domain.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAssignee.Commands.CreateTaskAssignee
{
    public class CreateTaskAssigneeCommand:IRequest<Response<bool>>
    {
        public Guid? TaskId { get; set; }
        public string? AssignedBy { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
