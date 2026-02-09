using Application.Common.Models;
using Application.Tasks.DTOs;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application.Tasks.Queries.GetTasks
{
    public class GetTasksQuery:IRequest<Response<List<TaskDTO>>>
    {
    }
}
