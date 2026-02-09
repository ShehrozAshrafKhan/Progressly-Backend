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

namespace Application.Tasks.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Response<List<TaskDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetTasksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<TaskDTO>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tasks = await _context.Tasks
                .Include(t => t.Attachments)
                .Include(t => t.Module)
                .Include(t => t.Assignees)
                    .ThenInclude(a => a.AssignedByUser) 
                    .Where(x=>x.IsActive==true)
                .Select(x => new TaskDTO()
                {
                    TaskId = x.TaskId,
                    Title = x.Title,
                    TaskNo = x.TaskNo,
                    Description = x.Description,
                    Status = x.Status,
                    Priority = x.Priority,
                    EstimatedHours = x.EstimatedHours,
                    IsActive = x.IsActive,
                    DueDate = x.DueDate,
                    ModuleId = x.ModuleId,
                    ModuleName = x.Module.ModuleName,
                    Attachments = x.Attachments.Select(a => new TaskAttachmentDTO
                    {
                        TaskAttachmentId = a.TaskAttachmentId,
                        FileName = GetProcessedFileName(a.FileName),
                        TaskId = a.TaskId
                    }).ToList(),
                    Assignees = x.Assignees.Select(b => new TaskAssigneeDTO
                    {
                        TaskId = b.TaskId,
                        AssignedAt = b.AssignedAt,
                        AssignedBy = b.AssignedBy,
                        TaskAssigneeId=b.TaskAssigneeId,
                        AssignedUserName = b.AssignedByUser != null ? b.AssignedByUser.FullName : null
                    }).ToList()
                }).ToListAsync(cancellationToken);



                if (tasks.Count == 0 || tasks == null)
                {
                    return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<List<TaskDTO>>() { result = Result.Success(), data = tasks };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
        private static string GetProcessedFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var parts = fileName.Split('-');
            return parts.Length > 1
                ? string.Join("-", parts.Skip(1))
                : fileName;
        }
    }

}
