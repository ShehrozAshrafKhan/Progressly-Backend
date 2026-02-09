using Application.GeneralSettings.Queries.GetApplicationName_Logo;
using Application.Reports.Queries.GetModules;
using Application.Reports.Queries.GetProjects;
using Application.Reports.Queries.GetTags;
using Application.Reports.Queries.GetTasks;
using Application.Reports.Queries.GetUserDateWiseTasks;
using Application.Reports.Queries.GetUserWiseTasks;
using Application.Reports.Queries.GetWeeklySummaryReport;
using AspNetCore.Reporting;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using LocalReport = Microsoft.Reporting.NETCore.LocalReport;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppSettingsService _appSettingsService;

        public ReportsController(IMediator mediator,IAppSettingsService appSettingsService)
        {
            _mediator = mediator;
            _appSettingsService = appSettingsService;
        }
        #region Projects

        [HttpGet("ProjectsReport")]
        public async Task<IActionResult> GetProjectsReport()
        {
            var generalSettings = await _appSettingsService.GetAppSettingsAsync();
            var result = await _mediator.Send(new GetProjectsQuery());

            if (!result.result.Succeeded || result.data == null)
            {
                return BadRequest(result.result.Errors);
            }

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "rpt_Projects.rdlc");
            using var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.LoadReportDefinition(reportStream);
            localReport.DataSources.Add(new ReportDataSource("rpt_Projects", result.data));
            var imagePath = new Uri(generalSettings.data.LogoPath).AbsoluteUri;
            localReport.SetParameters(new[]
            {
                  new ReportParameter("ProjectName", $"{generalSettings.data.ApplicationName}"),
                  new ReportParameter("ProjectLogo", $"{imagePath}")
             });
            var pdfBytes = localReport.Render("PDF");
            return File(pdfBytes, "application/pdf", "ProjectsReport.pdf");

        }

        #endregion

        #region Modules

        [HttpGet("ModulesReport")]
        public async Task<IActionResult> GetModulesReport()
        {
            var generalSettings = await _appSettingsService.GetAppSettingsAsync();
            var result = await _mediator.Send(new GetModulesQuery());

            if (!result.result.Succeeded || result.data == null)
            {
                return BadRequest(result.result.Errors);
            }

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "rpt_Modules.rdlc");
            using var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.LoadReportDefinition(reportStream);
            localReport.DataSources.Add(new ReportDataSource("rpt_Modules", result.data));
            var imagePath = new Uri(generalSettings.data.LogoPath).AbsoluteUri;
            localReport.SetParameters(new[]
            {
                  new ReportParameter("ProjectName", $"{generalSettings.data.ApplicationName}"),
                  new ReportParameter("ProjectLogo", $"{imagePath}")
            });
            var pdfBytes = localReport.Render("PDF");
            return File(pdfBytes, "application/pdf", "ModulesReport.pdf");

        }

        #endregion

        #region Tags

        [HttpGet("TagsReport")]
        public async Task<IActionResult> GetTagsReport()
        {
            var generalSettings = await _appSettingsService.GetAppSettingsAsync();
            var result = await _mediator.Send(new GetTagsQuery());

            if (!result.result.Succeeded || result.data == null)
            {
                return BadRequest(result.result.Errors);
            }

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "rpt_Tags.rdlc");
            using var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.LoadReportDefinition(reportStream);
            localReport.DataSources.Add(new ReportDataSource("rpt_Tags", result.data));
            var imagePath = new Uri(generalSettings.data.LogoPath).AbsoluteUri;
            localReport.SetParameters(new[]
            {
                 new ReportParameter("ProjectName", $"{generalSettings.data.ApplicationName}"),
                 new ReportParameter("ProjectLogo", $"{imagePath}")
            });
            var pdfBytes = localReport.Render("PDF");
            return File(pdfBytes, "application/pdf", "TagsReport.pdf");

        }

        #endregion

        #region Tasks

        [HttpGet("TasksReport")]
        public async Task<IActionResult> GetTasksReport()
        {
            var generalSettings = await _appSettingsService.GetAppSettingsAsync();
            var result = await _mediator.Send(new GetTasksQuery());

            if (!result.result.Succeeded || result.data == null)
            {
                return BadRequest(result.result.Errors);
            }

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "rpt_Tasks.rdlc");
            using var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.LoadReportDefinition(reportStream);
            localReport.DataSources.Add(new ReportDataSource("rpt_Tasks", result.data));
            var imagePath = new Uri(generalSettings.data.LogoPath).AbsoluteUri;
            localReport.SetParameters(new[]
            {
                 new ReportParameter("ProjectName", $"{generalSettings.data.ApplicationName}"),
                 new ReportParameter("ProjectLogo", $"{imagePath}")
            });
            var pdfBytes = localReport.Render("PDF");
            return File(pdfBytes, "application/pdf", "TasksReport.pdf");

        }
      
        #region UserWise

        [HttpGet("UsersWiseReport")]
        public async Task<IActionResult> GetUsersWiseReport(string? userId)
        {
            var generalSettings = await _appSettingsService.GetAppSettingsAsync();
            var result = await _mediator.Send(new GetUserWiseTasksQuery { UserId = userId });

            if (!result.result.Succeeded || result.data == null)
                return BadRequest(result.result.Errors);

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "rpt_UserTasks.rdlc");

            if (!System.IO.File.Exists(reportPath))
                return NotFound("Report file not found.");

            using var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.LoadReportDefinition(reportStream);

            localReport.DataSources.Add(new ReportDataSource("rpt_Tasks", result.data));

            var imagePath = new Uri(generalSettings.data.LogoPath).AbsoluteUri;
            localReport.SetParameters(new[]
            {
                 new ReportParameter("ProjectName", $"{generalSettings.data.ApplicationName}"),
                 new ReportParameter("ProjectLogo", $"{imagePath}")
            });

            var pdfBytes = localReport.Render("PDF");
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "UserTasksReport.pdf",
                Inline = true
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(pdfBytes, "application/pdf");
        }

        #endregion

        #region UserDateWise
        [HttpGet("UserDateWiseReport")]
        public async Task<IActionResult> GetUserDateWiseReport(string? userId,DateTime? fromDate,DateTime? toDate)
        {
            var generalSettings = await _appSettingsService.GetAppSettingsAsync();
            var result = await _mediator.Send(new GetUserDateWiseTasksQuery { UserId = userId, FromDate=fromDate, ToDate=toDate });

            if (!result.result.Succeeded || result.data == null)
                return BadRequest(result.result.Errors);

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "rpt_UserTasks.rdlc");

            if (!System.IO.File.Exists(reportPath))
                return NotFound("Report file not found.");

            using var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.LoadReportDefinition(reportStream);

            localReport.DataSources.Add(new ReportDataSource("rpt_Tasks", result.data));
            var imagePath = new Uri(generalSettings.data.LogoPath).AbsoluteUri;
            localReport.SetParameters(new[]
            {
                 new ReportParameter("ProjectName", $"{generalSettings.data.ApplicationName}"),
                 new ReportParameter("ProjectLogo", $"{imagePath}")
            });

            var pdfBytes = localReport.Render("PDF");
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "UserDateWiseTasksReport.pdf",
                Inline = true
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(pdfBytes, "application/pdf");
        }

        #endregion

        #region Weekly Summary Report

        [HttpGet("WeeklySummaryReport")]
        public async Task<IActionResult> GetWeeklySummaryReport(string? userId, DateTime? fromDate, DateTime? toDate)
        {
            var generalSettings = await _appSettingsService.GetAppSettingsAsync();
            var result = await _mediator.Send(new GetWeeklySummaryReportQuery { UserId = userId, FromDate = fromDate, ToDate = toDate});

            if (!result.result.Succeeded || result.data == null)
                return BadRequest(result.result.Errors);

            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "rpt_GetWeeklySummary.rdlc");

            if (!System.IO.File.Exists(reportPath))
                return NotFound("Report file not found.");

            using var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
            var localReport = new LocalReport();
            localReport.EnableExternalImages = true;
            localReport.LoadReportDefinition(reportStream);

            localReport.DataSources.Add(new ReportDataSource("rpt_GetWeeklySummary", result.data));

            var imagePath = new Uri(generalSettings.data.LogoPath).AbsoluteUri;
            localReport.SetParameters(new[]
            {
                 new ReportParameter("ProjectName", $"{generalSettings.data.ApplicationName}"),
                 new ReportParameter("ProjectLogo", $"{imagePath}")
            });

            var pdfBytes = localReport.Render("PDF");
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "WeeklySummaryReport.pdf",
                Inline = true
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(pdfBytes, "application/pdf");
        }
        #endregion

        #endregion


    }
}
