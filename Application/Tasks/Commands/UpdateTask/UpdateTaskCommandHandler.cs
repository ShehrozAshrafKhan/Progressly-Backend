using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTaskCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
           _context = context;
        }
        public async Task<Response<Guid>> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.TaskId == null || command.TaskId == Guid.Empty)
                {
                    return new Response<Guid>() { result = Result.Failure(new List<string>() { "TaskId not found!" }) };
                }
                var isTaskExisted = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == command.TaskId, cancellationToken);
                if (isTaskExisted == null)
                {
                    return new Response<Guid>() { result = Result.Failure(new List<string>() { "Task not found!" }) };
                }
                if (string.IsNullOrWhiteSpace(command.Title))
                {
                    return new Response<Guid>() { result = Result.Failure(new List<string>() { "Task Title is Required!" }) };
                }
                if (command.ModuleId == null || command.ModuleId == Guid.Empty)
                {
                    return new Response<Guid>() { result = Result.Failure(new List<string>() { "ModuleId not found!" }) };
                }
                var isModuleExisted = await _context.Modules.FirstOrDefaultAsync(x => x.ModuleId == command.ModuleId, cancellationToken);
                if (isModuleExisted == null)
                {
                    return new Response<Guid>() { result = Result.Failure(new List<string>() { "Module not found!" }) };

                }
                isTaskExisted.Title = command.Title;
                isTaskExisted.Priority = command.Priority;
                isTaskExisted.EstimatedHours = command.EstimatedHours;
                isTaskExisted.DueDate = command.DueDate??null;
                isTaskExisted.Status = command.Status;
                isTaskExisted.Description = command.Description;
                isTaskExisted.ModuleId = isModuleExisted.ModuleId;
                isTaskExisted.IsActive=command.IsActive;
                isTaskExisted.TaskNo = command.TaskNo;
                
                 _context.Tasks.Update(isTaskExisted);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<Guid>() { result = Result.Success(), data = isTaskExisted.TaskId };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<Guid>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
