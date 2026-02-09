using Application.Common.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ActivityLogger : IActivityLogger
    {
        private readonly IApplicationDbContext _context;

        public ActivityLogger(IApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task LogAsync(
             string? userId,
             string? userName,
             string action,
             string entity,
             string? details = null,
             Guid? taskId = null,
             string? description = null,
             TimeSpan? startTime = null,
             TimeSpan? endTime = null
         ,CancellationToken cancellationToken=default)
        {
            var log = new ActivityLog
            {
                ActivityLogId = Guid.NewGuid(),
                UserId = userId,
                UserName = userName,
                Action = action,
                Entity = entity,
                Details = details,
                TaskId = taskId,
                Description = description,
                StartTime = startTime,
                EndTime = endTime,
                ActivityDate = DateTime.UtcNow
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
