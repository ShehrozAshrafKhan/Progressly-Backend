using Application.Common.Interfaces;
using Domain;
using Domain.Common;
using Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,IApplicationDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
           _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Domain.Task> Tasks { get; set; }

        // === Task Related ===
        public DbSet<TaskUpdate> TaskUpdates { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<TaskAssignee> TaskAssignees { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<TaskCodeChange> TaskCodeChanges { get; set; }


        // === Notification & Tagging ===
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }
        public DbSet<ProjectAssignee> ProjectAssignees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseAuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
                }
            }
            modelBuilder.Entity<TaskAssignee>()
       .HasOne(t => t.AssignedByUser)
       .WithMany() // or .WithMany(u => u.TaskAssignees) if you have a collection in ApplicationUser
       .HasForeignKey(t => t.AssignedBy)
       .HasPrincipalKey(u => u.Id)  // assuming ApplicationUser.Id is string
       .OnDelete(DeleteBehavior.Restrict);
        }
        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var param = Expression.Parameter(type, "e");
            var prop = Expression.Property(param, nameof(BaseAuditableEntity.IsDeleted));
            var condition = Expression.Equal(prop, Expression.Constant(false));
            return Expression.Lambda(condition, param);
        }
        public override int SaveChanges()
        {
            ApplySoftDelete();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;

            var userName = user?.Identity?.Name ?? "System";
            var userId = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "System";

            foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.EnterBy = userName;
                    entry.Entity.EnterById = userId;
                    entry.Entity.Created = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedBy = userName;
                    entry.Entity.ModifiedById = userId;
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                }
            }

            ApplySoftDelete();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplySoftDelete()
        {
            var deletedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is BaseAuditableEntity);

            foreach (var entry in deletedEntries)
            {
                entry.State = EntityState.Modified;

                var entity = (BaseAuditableEntity)entry.Entity;
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                entity.DeletedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
            }
        }

        public static class ApplicationDbSeeder
        {
            private static readonly string[] Roles = new[] { "SUPER_ADMIN", "ADMIN", "USER", "MANAGER" };

            public static async System.Threading.Tasks.Task SeedAdminUserAsync(IServiceProvider services)
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var adminEmail = "admin@progressly.com";
                var adminPassword = "Admin123!";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        IsActive = true
                    };

                    var result = await userManager.CreateAsync(user, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "SUPER_ADMIN");
                    }
                }
            }

            public static async System.Threading.Tasks.Task SeedRolesAsync(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }

        public DbConnection GetDbConnection()
        {
            return Database.GetDbConnection();
        }
    }
}
