using Application.Common.Models;
using Application.TaskAssignee.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAssignee.Queries.GetTaskAssigneeById
{
    public class GetTaskAssigneeByIdQuery:IRequest<Response<TaskAssigneeDTO>>
    {
        public Guid? TaskAssigneeId { get; set; }
    }
}
