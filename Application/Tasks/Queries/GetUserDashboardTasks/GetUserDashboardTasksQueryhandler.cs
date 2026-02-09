using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskAssignee.DTOs;
using Application.TaskAttachments.DTOs;
using Application.Tasks.DTOs;
using Domain.Enums.Task;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.GetUserDashboardTasks
{
    public class GetUserDashboardTasksQueryhandler : IRequestHandler<GetUserDashboardTasksQuery, Response<List<Dictionary<string, int>>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetUserDashboardTasksQueryhandler(IApplicationDbContext context,ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Response<List<Dictionary<string, int>>>> Handle(GetUserDashboardTasksQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (_currentUser == null || string.IsNullOrEmpty(_currentUser.UserId))
                {
                    return new Response<List<Dictionary<string, int>>>()
                    {
                        result = Result.Failure(new List<string>() { "Current user reference not found!" })
                    };
                }

                var userId = _currentUser.UserId;

                var projectIds = await _context.ProjectAssignees
                    .Where(pa => pa.UserId == userId)
                    .Select(pa => pa.ProjectId)
                    .ToListAsync(cancellationToken);

                if (!projectIds.Any())
                {
                    return new Response<List<Dictionary<string, int>>>()
                    {
                        result = Result.Success(),
                        data = new List<Dictionary<string, int>>()
                        {
                            new Dictionary<string, int>() { { "TotalTasks", 0 } },
                            new Dictionary<string, int>() { { "MyTasks", 0 } },
                            new Dictionary<string, int>() { { "ActiveTasks", 0 } },
                            new Dictionary<string, int>() { { "CompletedTasks", 0 } },
                            new Dictionary<string, int>() { { "OverdueTasks", 0 } },
                            new Dictionary<string, int>() { { "UpcomingDeadlines", 0 } }
                        }
                    };
                }


                var moduleIds = await _context.Modules
                    .Where(m => projectIds.Contains(m.ProjectId??Guid.Empty))
                    .Select(m => m.ModuleId)
                    .ToListAsync(cancellationToken);

                if (!moduleIds.Any())
                {
                    return new Response<List<Dictionary<string, int>>>()
                    {
                        result = Result.Success(),
                        data = new List<Dictionary<string, int>>()
                {
                    new Dictionary<string, int>() { { "TotalTasks", 0 } },
                    new Dictionary<string, int>() { { "MyTasks", 0 } },
                    new Dictionary<string, int>() { { "ActiveTasks", 0 } },
                    new Dictionary<string, int>() { { "CompletedTasks", 0 } },
                    new Dictionary<string, int>() { { "OverdueTasks", 0 } },
                    new Dictionary<string, int>() { { "UpcomingDeadlines", 0 } }
                }
                    };
                }

                var totalTasksQuery = _context.Tasks
           .AsNoTracking()
           .Where(t => t.ModuleId.HasValue && moduleIds.Contains(t.ModuleId.Value));

                int totalTasks = await totalTasksQuery.CountAsync(cancellationToken);


                var assignedTaskIds = await _context.TaskAssignees
                    .Where(ta => ta.AssignedBy == userId)
                    .Select(ta => ta.TaskId)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                if (!assignedTaskIds.Any())
                {
                    return new Response<List<Dictionary<string, int>>>()
                    {
                        result = Result.Success(),
                        data = new List<Dictionary<string, int>>()
                {
                    new Dictionary<string, int>() { { "TotalTasks", totalTasks } },
                    new Dictionary<string, int>() { { "MyTasks", 0 } },
                    new Dictionary<string, int>() { { "ActiveTasks", 0 } },
                    new Dictionary<string, int>() { { "CompletedTasks", 0 } },
                    new Dictionary<string, int>() { { "OverdueTasks", 0 } },
                    new Dictionary<string, int>() { { "UpcomingDeadlines", 0 } }
                }
                    };
                }

                var tasksQuery = _context.Tasks
                    .AsNoTracking()
                    .Where(t => assignedTaskIds.Contains(t.TaskId));

                var now = DateTime.Now;

                int myTasks = await tasksQuery.CountAsync(cancellationToken);
                int activeTasks = await tasksQuery.Where(t => t.Status == Domain.Enums.Task.TaskStatus.IN_PROGRESS.ToString() && t.IsActive == true).CountAsync(cancellationToken);
                int completedTasks = await tasksQuery.Where(t => t.Status == Domain.Enums.Task.TaskStatus.COMPLETED.ToString() && t.IsActive == true).CountAsync(cancellationToken);
                int overdueTasks = await tasksQuery.Where(t => t.DueDate < now && t.Status != Domain.Enums.Task.TaskStatus.COMPLETED.ToString() && t.IsActive == true).CountAsync(cancellationToken);
                int upcomingTasks = await tasksQuery.Where(t => t.DueDate >= now && t.Status != Domain.Enums.Task.TaskStatus.COMPLETED.ToString() && t.IsActive == true).CountAsync(cancellationToken);

                var result = new List<Dictionary<string, int>>()
        {
            new Dictionary<string, int>() { { "TotalTasks", totalTasks } },
            new Dictionary<string, int>() { { "MyTasks", myTasks } },
            new Dictionary<string, int>() { { "ActiveTasks", activeTasks } },
            new Dictionary<string, int>() { { "CompletedTasks", completedTasks } },
            new Dictionary<string, int>() { { "OverdueTasks", overdueTasks } },
            new Dictionary<string, int>() { { "UpcomingDeadlines", upcomingTasks } }
        };

                return new Response<List<Dictionary<string, int>>>() { data = result, result = Result.Success() };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<Dictionary<string, int>>> () { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
