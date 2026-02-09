using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Progressly.Application.DocumentStore.Commands.UploadFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAttachments.Commands.UpdateTaskAttachment
{
    public class UpdateTaskAttachmentCommandHandler : IRequestHandler<UpdateTaskAttachmentCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UploadFileCommandHandler _uploadFileCommandHandler;

        public UpdateTaskAttachmentCommandHandler(
            IApplicationDbContext context,
            UploadFileCommandHandler uploadFileCommandHandler)
        {
            _context = context;
            _uploadFileCommandHandler = uploadFileCommandHandler;
        }

        public async Task<Response<bool>> Handle(UpdateTaskAttachmentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.TaskAttachmentId == null)
                {
                    return new Response<bool>{result = Result.Failure(new List<string> { "TaskAttachmentId is required." })};
                }
                var attachment = await _context.TaskAttachments.FirstOrDefaultAsync(x => x.TaskAttachmentId == command.TaskAttachmentId, cancellationToken);
                if (attachment == null)
                {
                    return new Response<bool>{result = Result.Failure(new List<string> { "TaskAttachment not found." })};
                }
                if (command.TaskId != null)
                {
                    var taskExists = await _context.Tasks.AnyAsync(t => t.TaskId == command.TaskId, cancellationToken);
                    if (!taskExists)
                    {
                        return new Response<bool>{result = Result.Failure(new List<string> { "Specified Task not found." })};
                    }
                    attachment.TaskId = command.TaskId;
                }
                if (command.File != null)
                {
                    var fileResult = await _uploadFileCommandHandler.HandleAsync(new UploadFileCommand { File = command.File });
                    if (fileResult.Item1 == null)
                    {
                        return new Response<bool>{result = Result.Failure(new List<string> { "File upload failed!" })};
                    }
                    attachment.FileName = fileResult.Item1;
                    attachment.FilePath = fileResult.Item2 ?? "";
                }

                _context.TaskAttachments.Update(attachment);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool> { result = Result.Success(), data = true };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return new Response<bool>{result = Result.Failure(new List<string> { message })};
            }
        }
    }
}
