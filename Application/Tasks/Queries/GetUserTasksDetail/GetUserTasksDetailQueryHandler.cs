using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskAssignee.DTOs;
using Application.TaskAttachments.DTOs;
using Application.Tasks.DTOs;
using Domain;
using Domain.Enums.Task;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.GetUserTasksDetail
{
    public class GetUserTasksDetailQueryHandler : IRequestHandler<GetUserTasksDetailQuery, Response<List<TaskDTO>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetUserTasksDetailQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Response<List<TaskDTO>>> Handle(GetUserTasksDetailQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (_currentUser == null || string.IsNullOrEmpty(_currentUser.UserId))
                {
                    return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "Current user reference not found!" }) };
                }
                var userId = _currentUser.UserId;
                if (string.IsNullOrWhiteSpace(query.Type))
                {
                    return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "Task Type not found!" }) };
                }
                var projectIds = await _context.ProjectAssignees
                                   .Where(pa => pa.UserId == userId)
                                   .Select(pa => pa.ProjectId)
                                   .ToListAsync(cancellationToken);
                if (projectIds.Count == 0 || projectIds == null)
                {
                    return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "Project not assigned!" }) };
                }
                var moduleIds = await _context.Modules
                    .Where(m => projectIds.Contains(m.ProjectId ?? Guid.Empty))
                    .Select(m => m.ModuleId)
                    .ToListAsync(cancellationToken);
                if (moduleIds.Count == 0 || moduleIds == null)
                {
                    return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "Module not assigned to these projects!" }) };
                }
                var totalTasksQuery = _context.Tasks.AsNoTracking().Include(t => t.Attachments)
               .Include(t => t.Module)
               .Include(t => t.Assignees)
                   .ThenInclude(a => a.AssignedByUser)
                .Where(t => t.ModuleId.HasValue && moduleIds.Contains(t.ModuleId.Value));

                var assignedTaskIds = await _context.TaskAssignees
                  .Where(ta => ta.AssignedBy == userId)
                  .Select(ta => ta.TaskId)
                  .Distinct()
                  .ToListAsync(cancellationToken);

                var tasksQuery = _context.Tasks
                  .AsNoTracking()
                    .Include(t => t.Module)
                    .Include(t => t.Assignees)
                      .ThenInclude(a => a.AssignedByUser)
                  .Where(t => assignedTaskIds.Contains(t.TaskId));

                var now = DateTime.Now;

                if (query.Type == TaskType.ALL.ToString())
                {
                    var totalTasks = await totalTasksQuery
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
                             TaskAssigneeId = b.TaskAssigneeId,
                             AssignedUserName = b.AssignedByUser != null ? b.AssignedByUser.FullName : null
                         }).ToList()
                     }).ToListAsync(cancellationToken);
                    if (totalTasks.Count == 0 || totalTasks == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    return new Response<List<TaskDTO>>() { result = Result.Success(), data = totalTasks };
                }
                else if (query.Type == TaskType.MYTASK.ToString())
                {
                    if (assignedTaskIds.Count == 0 || assignedTaskIds == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    var myTasks = await tasksQuery
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
                             TaskAssigneeId = b.TaskAssigneeId,
                             AssignedUserName = b.AssignedByUser != null ? b.AssignedByUser.FullName : null
                         }).ToList()
                     }).ToListAsync(cancellationToken);
                    if (myTasks.Count == 0 || myTasks == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    return new Response<List<TaskDTO>>() { result = Result.Success(), data = myTasks };
                }
                else if (query.Type == TaskType.ACTIVE.ToString())
                {
                    if (assignedTaskIds.Count == 0 || assignedTaskIds == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    var myTasks = await tasksQuery.Where(t => t.IsActive == true)
                        .Where(x=>x.Status==Domain.Enums.Task.TaskStatus.IN_PROGRESS.ToString())
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
                             TaskAssigneeId = b.TaskAssigneeId,
                             AssignedUserName = b.AssignedByUser != null ? b.AssignedByUser.FullName : null
                         }).ToList()
                     }).ToListAsync(cancellationToken);
                    if (myTasks.Count == 0 || myTasks == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    return new Response<List<TaskDTO>>() { result = Result.Success(), data = myTasks };
                }
                else if (query.Type == TaskType.COMPLETED.ToString())
                {
                    if (assignedTaskIds.Count == 0 || assignedTaskIds == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    var completedTasks = await tasksQuery.Where(t => t.IsActive == true)
                        .Where(x=>x.Status==Domain.Enums.Task.TaskStatus.COMPLETED.ToString())
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
                             TaskAssigneeId = b.TaskAssigneeId,
                             AssignedUserName = b.AssignedByUser != null ? b.AssignedByUser.FullName : null
                         }).ToList()
                     }).ToListAsync(cancellationToken);
                    if (completedTasks.Count == 0 || completedTasks == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    return new Response<List<TaskDTO>>() { result = Result.Success(), data = completedTasks };
                }
                else if (query.Type == TaskType.UPCOMING.ToString())
                {
                    if (assignedTaskIds.Count == 0 || assignedTaskIds == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    var upcomingTasks = await tasksQuery.Where(t => t.IsActive == true)
                        .Where(x=>x.Status!=Domain.Enums.Task.TaskStatus.COMPLETED.ToString()&&x.DueDate >= now)
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
                             TaskAssigneeId = b.TaskAssigneeId,
                             AssignedUserName = b.AssignedByUser != null ? b.AssignedByUser.FullName : null
                         }).ToList()
                     }).ToListAsync(cancellationToken);
                    if (upcomingTasks.Count == 0 || upcomingTasks == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    return new Response<List<TaskDTO>>() { result = Result.Success(), data = upcomingTasks };
                }
                else if (query.Type == TaskType.OVERDUE.ToString())
                {
                    if (assignedTaskIds.Count == 0 || assignedTaskIds == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    var overdueTasks = await tasksQuery.Where(t => t.IsActive == true)
                        .Where(x=>x.Status!=Domain.Enums.Task.TaskStatus.COMPLETED.ToString()&&x.DueDate < now)
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
                             TaskAssigneeId = b.TaskAssigneeId,
                             AssignedUserName = b.AssignedByUser != null ? b.AssignedByUser.FullName : null
                         }).ToList()
                     }).ToListAsync(cancellationToken);
                    if (overdueTasks.Count == 0 || overdueTasks == null)
                    {
                        return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                    }
                    return new Response<List<TaskDTO>>() { result = Result.Success(), data = overdueTasks };
                }
                else
                {
                    return new Response<List<TaskDTO>>() { result = Result.Failure(new List<string>() { "TaskType not found!" }) };
                }

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
