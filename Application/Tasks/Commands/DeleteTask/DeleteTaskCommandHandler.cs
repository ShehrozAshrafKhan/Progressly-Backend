using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == command.TaskId, cancellationToken);
                if (task == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string> { "Task not found!" }) };
                }
                _context.Tasks.Remove(task);
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
