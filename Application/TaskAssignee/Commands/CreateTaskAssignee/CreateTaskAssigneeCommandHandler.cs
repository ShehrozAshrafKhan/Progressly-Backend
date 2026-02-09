using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.TaskAssignee.Commands.CreateTaskAssignee
{
    public class CreateTaskAssigneeCommandHandler : IRequestHandler<CreateTaskAssigneeCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateTaskAssigneeCommandHandler(IApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Response<bool>> Handle(CreateTaskAssigneeCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.TaskId == null || command.TaskId == Guid.Empty)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "TaskId not found!" }) };
                }
                var isTaskExisted = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == command.TaskId, cancellationToken);
                if (isTaskExisted == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "Task not found!" }) };

                }
                if (string.IsNullOrWhiteSpace(command.AssignedBy))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "UserId not found!" }) };
                }
                var isUserExisted = await  _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.AssignedBy, cancellationToken);
                if (isUserExisted == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "User not found!" }) };

                }
                var taskAssignee = new Domain.TaskAssignee()
                {
                   TaskId=isTaskExisted.TaskId,
                   AssignedBy=isUserExisted.Id,
                   AssignedAt=command.AssignedAt
                };
                await _context.TaskAssignees.AddAsync(taskAssignee);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>() { result = Result.Success(), data = true };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
