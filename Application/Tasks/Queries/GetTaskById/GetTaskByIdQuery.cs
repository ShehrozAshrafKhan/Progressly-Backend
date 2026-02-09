using Application.Common.Models;
using Application.Tasks.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQuery:IRequest<Response<TaskDTO>>
    {
        public Guid? TaskId { get; set; }
    }
}
