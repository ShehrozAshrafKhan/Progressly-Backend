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

namespace Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _context;

        public CreateTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<Guid>> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {
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
                var task = new Domain.Task()
                {
                    Title = command.Title,
                    Priority = command.Priority,
                    EstimatedHours=command.EstimatedHours,
                    DueDate=command.DueDate??null,
                    Status = command.Status,
                    TaskNo=command.TaskNo??null,
                    Description = command.Description,
                    ModuleId = isModuleExisted.ModuleId,
                    IsActive=command.IsActive
                };
                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<Guid>() { result = Result.Success(), data = task.TaskId };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<Guid>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
