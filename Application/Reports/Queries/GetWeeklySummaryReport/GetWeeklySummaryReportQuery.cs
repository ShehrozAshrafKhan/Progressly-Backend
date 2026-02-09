using Application.Common.Models;
using Application.Reports.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.Queries.GetWeeklySummaryReport
{
    public class GetWeeklySummaryReportQuery : IRequest<Response<List<WeeklySummaryReportDTO>>>
    {
        public string? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
