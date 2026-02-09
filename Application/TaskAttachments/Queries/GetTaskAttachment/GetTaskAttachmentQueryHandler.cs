using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskAttachments.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAttachments.Queries.GetTaskAttachment
{
    public class GetTaskAttachmentQueryHandler : IRequestHandler<GetTaskAttachmentQuery, Response<List<TaskAttachmentDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetTaskAttachmentQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<TaskAttachmentDTO>>> Handle(GetTaskAttachmentQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var taskAttachments = await _context.TaskAttachments.Select(x => new TaskAttachmentDTO
                {
                    FileName = x.FileName,
                    TaskAttachmentId = x.TaskAttachmentId,
                    TaskId = x.TaskId
                }).ToListAsync();

                if (taskAttachments.Count == 0||taskAttachments==null)
                {
                    return new Response<List<TaskAttachmentDTO>>() { result = Result.Failure(new List<string>() { "No Data Found!" }) };
                }
                return new Response<List<TaskAttachmentDTO>>() { result=Result.Success(),data=taskAttachments};
            }
            catch (Exception ex) {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<TaskAttachmentDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
