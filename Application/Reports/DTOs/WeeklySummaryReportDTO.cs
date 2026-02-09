using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.DTOs
{
    public class WeeklySummaryReportDTO
    {
        public int? Year { get; set; }
        public int? WeekNumber { get; set; }
        public DateTime? WeekStartDate { get; set; }
        public DateTime? WeekEndDate { get; set; }
        public int? TotalTasks { get; set; }
        public int? CompletedTasks { get; set; }
        public int? PendingTasks { get; set; }
        public int? OverdueTasks { get; set; }
    }
}
