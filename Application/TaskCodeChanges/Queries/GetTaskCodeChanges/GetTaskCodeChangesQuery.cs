using Application.Common.Models;
using Application.TaskCodeChanges.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.Queries.GetTaskCodeChanges
{
    public class GetTaskCodeChangesQuery:IRequest<Response<List<TaskCodeChangesDTO>>>
    {
        public Guid? TaskId { get; set; }
    }
}
