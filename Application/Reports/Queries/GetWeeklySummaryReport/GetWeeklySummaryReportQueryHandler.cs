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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Reports.Queries.GetWeeklySummaryReport
{
    public class GetWeeklySummaryReportQueryHandler : IRequestHandler<GetWeeklySummaryReportQuery, Response<List<WeeklySummaryReportDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetWeeklySummaryReportQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<WeeklySummaryReportDTO>>> Handle(GetWeeklySummaryReportQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var parameters = new { UserId = query.UserId, FromDate = query.FromDate, ToDate = query.ToDate };
                using (var connection = _context.GetDbConnection())
                {
                    var result = await connection.QueryMultipleAsync("WeeklySummaryReport", parameters, commandType: CommandType.StoredProcedure);

                    var tasks = (await result.ReadAsync<WeeklySummaryReportDTO>()).ToList();
                    return new Response<List<WeeklySummaryReportDTO>>() { result = Result.Success(), data = tasks };
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<WeeklySummaryReportDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
