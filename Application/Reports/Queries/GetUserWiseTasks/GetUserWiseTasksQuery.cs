using Application.Common.Models;
using Application.Reports.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.Queries.GetUserWiseTasks
{
    public class GetUserWiseTasksQuery:IRequest<Response<List<TaskReportDTO>>>
    {
        public string? UserId { get; set; }
    }
}
