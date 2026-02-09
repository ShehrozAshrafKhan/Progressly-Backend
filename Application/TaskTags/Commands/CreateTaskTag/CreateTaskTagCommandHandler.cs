using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TaskTags.Commands.CreateTaskTag
{
    public class CreateTaskTagCommandHandler : IRequestHandler<CreateTaskTagCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public CreateTaskTagCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<bool>> Handle(CreateTaskTagCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.TaskId == null || command.TagIds == null || !command.TagIds.Any())
                {
                    return new Response<bool>()
                    {
                        result = Result.Failure(new List<string>() { "TaskId and at least one TagId are required." })
                    };
                }
                var existingTags = await _context.TaskTags.Where(tt => tt.TaskId == command.TaskId).ToListAsync(cancellationToken);
                _context.TaskTags.RemoveRange(existingTags);
                foreach (var tagId in command.TagIds)
                {
                    var taskTag = new TaskTag
                    {
                        TaskId = command.TaskId.Value,
                        TagId = tagId
                    };
                    await _context.TaskTags.AddAsync(taskTag, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>(){  result = Result.Success(),data = true};
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null? ex.Message: string.IsNullOrEmpty(ex.InnerException.Message)? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>(){result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
