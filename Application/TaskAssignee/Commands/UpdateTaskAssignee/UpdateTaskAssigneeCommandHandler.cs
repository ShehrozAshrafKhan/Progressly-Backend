using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TaskAssignee.Commands.UpdateTaskAssignee
{
    public class UpdateTaskAssigneeCommandHandler : IRequestHandler<UpdateTaskAssigneeCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateTaskAssigneeCommandHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Response<bool>> Handle(UpdateTaskAssigneeCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.TaskId == null || command.TaskId == Guid.Empty)
                {
                    return new Response<bool> { result = Result.Failure(new List<string> { "TaskId is required." }) };
                }

                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == command.TaskId, cancellationToken);
                if (task == null)
                {
                    return new Response<bool> { result = Result.Failure(new List<string> { "Task not found." }) };
                }

                var existingAssignees = await _context.TaskAssignees
                    .Where(x => x.TaskId == command.TaskId)
                    .ToListAsync(cancellationToken);

                _context.TaskAssignees.RemoveRange(existingAssignees);

                if (command.AssignedBy != null && command.AssignedBy.Count > 0)
                {
                    var distinctUserIds = command.AssignedBy.Distinct();

                    foreach (var userId in distinctUserIds)
                    {
                        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

                        if (user != null)
                        {
                            var newAssignee = new Domain.TaskAssignee
                            {
                                TaskId = task.TaskId,
                                AssignedBy = user.Id,
                                AssignedAt = command.AssignedAt ?? DateTime.UtcNow
                            };
                            await _context.TaskAssignees.AddAsync(newAssignee, cancellationToken);
                        }
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new Response<bool> { result = Result.Success(), data = true };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<bool> { result = Result.Failure(new List<string> { message }) };
            }
        }

    }
}

