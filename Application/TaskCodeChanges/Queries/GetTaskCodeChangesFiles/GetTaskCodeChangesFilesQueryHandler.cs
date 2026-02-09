using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskAttachments.Queries.GetTaskAttachmentFiles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Progressly.Application.DocumentStore.DTOs;
using Progressly.Application.DocumentStore.Queries.GetFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.Queries.GetTaskCodeChangesFiles
{
    public class GetTaskCodeChangesFilesQueryHandler : IRequestHandler<GetTaskCodeChangesFilesQuery, Response<FileInfoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly GetFileQueryHandler _getFileQueryHandler;

        public GetTaskCodeChangesFilesQueryHandler(IApplicationDbContext context, GetFileQueryHandler getFileQueryHandler)
        {
            _context = context;
            _getFileQueryHandler = getFileQueryHandler;
        }
        public async Task<Response<FileInfoDto>> Handle(GetTaskCodeChangesFilesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (query.TaskCodeChangeId == Guid.Empty || query.TaskCodeChangeId == null)
                {
                    return new Response<FileInfoDto>(){result = Result.Failure(new List<string>() { "TaskCodeChangeId not found!" })};
                }
                var change = await _context.TaskCodeChanges.FirstOrDefaultAsync(x => x.TaskCodeChangeId == query.TaskCodeChangeId, cancellationToken);
                if (change == null)
                {
                    return new Response<FileInfoDto>(){result = Result.Failure(new List<string>() { "TaskCodeChange not found!" })};
                }
                string? matchedFilePath = null;

                if (!string.IsNullOrEmpty(query.FileName))
                {
                    if (query.FileType == "OLD")
                    {
                        if (query.FileName.Equals(change.OldFileName + change.OldExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            matchedFilePath = change.OldFilePath;
                        }
                    }
                    else if (query.FileType == "NEW")
                    {
                        if (query.FileName.Equals(change.NewFileName + change.NewExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            matchedFilePath = change.NewFilePath;
                        }
                    }
                }
                if (string.IsNullOrEmpty(matchedFilePath))
                {
                    return new Response<FileInfoDto>(){ result = Result.Failure(new List<string>() { "Requested file does not match old or new file name." })};
                }
                var fileInfoDTO = await _getFileQueryHandler.Handle(new GetFileQuery(){ FilePath = matchedFilePath, FileName = query.FileName ?? "" });
                if (fileInfoDTO == null)
                {
                    return new Response<FileInfoDto>() {result = Result.Failure(new List<string>() { "File not found on disk." })};
                }
                return new Response<FileInfoDto>() {result = Result.Success(),data = fileInfoDTO};
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null? ex.Message: ex.InnerException.Message ?? ex.InnerException.ToString();
                return new Response<FileInfoDto>() {result = Result.Failure(new List<string>() { message }) };
            }
        }

    }
}
