using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Reports.DTOs;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.Queries.GetUserDateWiseTasks
{
    public class GetUserDateWiseTasksQueryHandler : IRequestHandler<GetUserDateWiseTasksQuery, Response<List<TaskReportDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetUserDateWiseTasksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<TaskReportDTO>>> Handle(GetUserDateWiseTasksQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var parameters = new { UserId = query.UserId,FromDate=query.FromDate,ToDate=query.ToDate };
                using (var connection = _context.GetDbConnection())
                {
                    var result = await connection.QueryMultipleAsync("GetUserTasks", parameters, commandType: CommandType.StoredProcedure);

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
