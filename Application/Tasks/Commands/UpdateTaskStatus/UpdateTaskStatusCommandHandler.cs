using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums.Task;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using TaskStatus = Domain.Enums.Task.TaskStatus;

namespace Application.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTaskStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(UpdateTaskStatusCommand command, CancellationToken cancellationToken)
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
                if (isTaskExisted.Status==TaskStatus.COMPLETED.ToString())
                {
                    isTaskExisted.IsCompletedRequest = command.IsCompletedRequest;

                    _context.Tasks.Update(isTaskExisted);
                    await _context.SaveChangesAsync(cancellationToken);
                    return new Response<bool>() { result = Result.Success(), data = true };
                }
                else
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "Task is not completed yet!" }) };
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
