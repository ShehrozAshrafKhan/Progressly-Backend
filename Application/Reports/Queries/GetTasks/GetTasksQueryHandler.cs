using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskAssignee.DTOs;
using Application.TaskAttachments.DTOs;
using Application.Tasks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Application.Reports.DTOs;
using Dapper;
using System.Data;

namespace Application.Reports.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Response<List<TaskReportDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetTasksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<TaskReportDTO>>> Handle(GetTasksQuery query, CancellationToken cancellationToken)
        {
            try
            {
                //var parameters = new { TaskId = query.TaskId };
                using (var connection = _context.GetDbConnection())
                {
                    var result = await connection.QueryMultipleAsync("GetTasks", null, commandType: CommandType.StoredProcedure);

                    var tasks = (await result.ReadAsync<TaskReportDTO>()).ToList();
                    return new Response<List<TaskReportDTO>>() { result = Result.Success(), data = tasks };
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<TaskReportDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }

    }

}
