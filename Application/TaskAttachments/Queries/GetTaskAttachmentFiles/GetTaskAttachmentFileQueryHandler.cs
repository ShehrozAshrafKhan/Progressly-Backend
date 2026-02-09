using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Progressly.Application.DocumentStore.DTOs;
using Progressly.Application.DocumentStore.Queries.GetFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAttachments.Queries.GetTaskAttachmentFiles
{
    public class GetTaskAttachmentFileQueryHandler : IRequestHandler<GetTaskAttachmentFileQuery, Response<FileInfoDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly GetFileQueryHandler _getFileQueryHandler;

        public GetTaskAttachmentFileQueryHandler(IApplicationDbContext context, GetFileQueryHandler getFileQueryHandler)
        {
           _context = context;
           _getFileQueryHandler = getFileQueryHandler;
        }
        public async Task<Response<FileInfoDto>> Handle(GetTaskAttachmentFileQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (query.TaskAttachmentId==Guid.Empty||query.TaskAttachmentId==null)
                {
                    return new Response<FileInfoDto>() { result = Result.Failure(new List<string>() { "TaskAttachmentId Not Found!" }) };
                }
                var isTaskAttachmentExisted = await _context.TaskAttachments.FirstOrDefaultAsync(x => x.TaskAttachmentId == query.TaskAttachmentId);
                if (isTaskAttachmentExisted==null)
                {
                    return new Response<FileInfoDto>() { result = Result.Failure(new List<string>() { "TaskAttachment Not Found!" }) };
                }
                var originalFileName = isTaskAttachmentExisted.FileName;
                var displayFileName = string.IsNullOrWhiteSpace(originalFileName) || !originalFileName.Contains('-')
                    ? originalFileName ?? ""
                    : originalFileName.Substring(originalFileName.IndexOf('-') + 1);

                var fileInfoDTO = await _getFileQueryHandler.Handle(new GetFileQuery() { FilePath = isTaskAttachmentExisted.FilePath ?? "", FileName = displayFileName ?? "" });
                if (fileInfoDTO == null)
                {
                    return new Response<FileInfoDto>() { result = Result.Failure(new List<string>() { "File not exist!" }) };
                }
                return new Response<FileInfoDto>() { result = Result.Success(), data = fileInfoDTO };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<FileInfoDto>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
