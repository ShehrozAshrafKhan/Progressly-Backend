using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<GeneralSetting> GeneralSettings { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Domain.Module> Modules { get; set; }
        DbSet<Domain.Task> Tasks { get; set; }

        // === Task Related ===
        DbSet<TaskUpdate> TaskUpdates { get; set; }
        DbSet<TaskAttachment> TaskAttachments { get; set; }
        DbSet<Domain.TaskAssignee> TaskAssignees { get; set; }
        DbSet<ActivityLog> ActivityLogs { get; set; }

        // === Notification & Tagging ===
        DbSet<Notification> Notifications { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<TaskTag> TaskTags { get; set; }
        DbSet<ProjectAssignee> ProjectAssignees { get; set; }
        DbSet<TaskCodeChange> TaskCodeChanges { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
        DbConnection GetDbConnection();
    }
}
