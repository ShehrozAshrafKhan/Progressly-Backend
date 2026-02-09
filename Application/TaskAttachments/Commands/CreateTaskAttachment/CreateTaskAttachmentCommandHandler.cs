using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Progressly.Application.DocumentStore.Commands.UploadFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAttachments.Commands.CreateTaskAttachment
{
    public class CreateTaskAttachmentCommandHandler : IRequestHandler<CreateTaskAttachmentCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UploadFileCommandHandler _uploadFileCommandHandler;

        public CreateTaskAttachmentCommandHandler(IApplicationDbContext context,UploadFileCommandHandler uploadFileCommandHandler)
        {
            _context = context;
            _uploadFileCommandHandler = uploadFileCommandHandler;
        }
        public async Task<Response<bool>> Handle(CreateTaskAttachmentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.File==null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "File is null or Empty" }) };
                }
                var isTaskExisted = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == command.TaskId, cancellationToken);
                if (isTaskExisted==null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "Task Not Found!" }) };
                }
                var fileResult = await _uploadFileCommandHandler.HandleAsync(new UploadFileCommand { File = command.File });
                if (fileResult.Item1==null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "File upload failed!" }) };
                }
                var taskAttchment = new TaskAttachment()
                {
                    TaskId = command.TaskId,
                    FilePath = fileResult.Item2??"",
                    FileName = fileResult.Item1
                };
                await _context.TaskAttachments.AddAsync(taskAttchment);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>() { result = Result.Success(),data=true };
            }
            catch (Exception ex) {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
