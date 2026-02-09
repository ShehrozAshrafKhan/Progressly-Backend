using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Progressly.Application.DocumentStore.Commands.UploadFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.Commands.UploadCodeChanges
{
    public class UploadCodeChangesCommandHandler : IRequestHandler<UploadCodeChangesCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UploadFileCommandHandler _uploadFileCommandHandler;

        public UploadCodeChangesCommandHandler(IApplicationDbContext context, UploadFileCommandHandler uploadFileCommandHandler)
        {
            _context = context;
            _uploadFileCommandHandler = uploadFileCommandHandler;
        }

        public async Task<Response<bool>> Handle(UploadCodeChangesCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var formData = command.FormData;
                var taskIdString = formData["taskId"].ToString();
                if (!Guid.TryParse(taskIdString, out Guid taskId))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string> { "Invalid taskId" }) };
                }
                var codeChanges = new List<TaskCodeChange>();

                var groupedFiles = formData.Files.GroupBy(f => f.Name.Split("_")[0] + "_" + f.Name.Split("_")[1]); // e.g. tagId_0

                foreach (var group in groupedFiles)
                {
                    var prefix = group.Key;
                    var tagIdString = formData[$"{prefix}_tagId"].ToString();
                    var entryType = formData[$"{prefix}_type"].ToString();

                    if (!Guid.TryParse(tagIdString, out Guid tagId)) continue;

                    var oldFile = group.FirstOrDefault(f => f.Name == $"{prefix}_oldFile");
                    var newFile = group.FirstOrDefault(f => f.Name == $"{prefix}_newFile");

                    var change = new TaskCodeChange
                    {
                        TaskId = taskId,
                        TagId = tagId,
                        EntryType = entryType
                    };

                    var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Tasks", taskId.ToString(), prefix);
                    Directory.CreateDirectory(storagePath);

                    // Save Old File
                    if (oldFile != null)
                    {
                        var prefixedOldFileName = $"old_{oldFile.FileName}";
                        var path = Path.Combine(storagePath, prefixedOldFileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                            await oldFile.CopyToAsync(stream, cancellationToken);

                        change.OldFileName = Path.GetFileNameWithoutExtension(oldFile.FileName);
                        change.OldExtension = Path.GetExtension(oldFile.FileName);
                        change.OldFilePath = path.Replace(Directory.GetCurrentDirectory(), "").Replace("\\", "/");
                    }

                    // Save New File
                    if (newFile != null)
                    {
                        var prefixedNewFileName = $"new_{newFile.FileName}";
                        var path = Path.Combine(storagePath, prefixedNewFileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                            await newFile.CopyToAsync(stream, cancellationToken);

                        change.NewFileName = Path.GetFileNameWithoutExtension(newFile.FileName);
                        change.NewExtension = Path.GetExtension(newFile.FileName);
                        change.NewFilePath = path.Replace(Directory.GetCurrentDirectory(), "").Replace("\\", "/");
                    }
                    codeChanges.Add(change);
                }

                await _context.TaskCodeChanges.AddRangeAsync(codeChanges, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>() { result = Result.Success(), data = true };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : string.IsNullOrEmpty(ex.InnerException.Message) ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
