using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IActivityLogger
    {
        Task LogAsync(
             string? userId,
             string? userName,
             string action,
             string entity,
             string? details = null,
             Guid? taskId = null,
             string? description = null,
             TimeSpan? startTime = null,
             TimeSpan? endTime = null,
             CancellationToken cancellationToken = default
         );
    }
}
