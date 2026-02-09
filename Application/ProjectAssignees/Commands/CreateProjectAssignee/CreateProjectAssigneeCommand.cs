using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProjectAssignees.Commands.CreateProjectAssignee
{
    public class CreateProjectAssigneeCommand:IRequest<Response<bool>>
    {
        public Guid? ProjectId { get; set; }
        public string? UserId { get; set; }
    }
}
