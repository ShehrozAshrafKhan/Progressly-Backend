using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.Commands.DeleteTaskCodeChange
{
    public class DeleteTaskCodeChangeCommandHandler : IRequestHandler<DeleteTaskCodeChangeCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTaskCodeChangeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(DeleteTaskCodeChangeCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var taskCodeChange = await _context.TaskCodeChanges.FirstOrDefaultAsync(x => x.TaskCodeChangeId == command.TaskCodeChangeId, cancellationToken);
                if (taskCodeChange == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string> { "TaskCodeChange not found!" }) };
                }
                if (!string.IsNullOrEmpty(taskCodeChange.OldFilePath))
                {
                    var oldFullPath = Path.Combine(Directory.GetCurrentDirectory(), taskCodeChange.OldFilePath.TrimStart('/').Replace("/", "\\"));
                    if (System.IO.File.Exists(oldFullPath))
                        System.IO.File.Delete(oldFullPath);
                }

                if (!string.IsNullOrEmpty(taskCodeChange.NewFilePath))
                {
                    var newFullPath = Path.Combine(Directory.GetCurrentDirectory(), taskCodeChange.NewFilePath.TrimStart('/').Replace("/", "\\"));
                    if (System.IO.File.Exists(newFullPath))
                        System.IO.File.Delete(newFullPath);
                }
                _context.TaskCodeChanges.Remove(taskCodeChange);
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
