using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskAssignee.DTOs;
using Application.TaskAttachments.DTOs;
using Application.Tasks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Response<TaskDTO>>
    {
        private readonly IApplicationDbContext _context;

        public GetTaskByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<TaskDTO>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _context.Tasks
                .Include(t => t.Attachments)
                .Include(t => t.Module)
                .Include(t=>t.TaskTags)
                .Include(t => t.Assignees)
                    .ThenInclude(a => a.AssignedByUser)
                .Select(x => new TaskDTO()
                {
                    TaskId = x.TaskId,
                    Title = x.Title,
                    TaskNo = x.TaskNo,
                    Description = x.Description,
                    Status = x.Status,
                    Priority = x.Priority,
                    EstimatedHours = x.EstimatedHours,
                    IsActive = x.IsActive,
                    DueDate = x.DueDate,
                    ModuleId = x.ModuleId,
                    ModuleName = x.Module.ModuleName,
                    IsCompletedRequest=x.IsCompletedRequest,
                    Attachments = x.Attachments.Select(a => new TaskAttachmentDTO
                    {
                        TaskAttachmentId = a.TaskAttachmentId,
                        FileName = GetProcessedFileName(a.FileName),
                        TaskId = a.TaskId
                    }).ToList(),
                    Assignees = x.Assignees.Select(b => new TaskAssigneeDTO
                    {
                        TaskId = b.TaskId,
                        AssignedAt = b.AssignedAt,
                        AssignedBy = b.AssignedBy,
                        TaskAssigneeId = b.TaskAssigneeId,
                        AssignedUserName = b.AssignedByUser != null ? b.AssignedByUser.FullName : null
                    }).ToList(),
                    TaskTags=x.TaskTags.Select(c=>new TaskTags.DTOs.TaskTagDTO
                    {
                        TagId=c.TagId,
                        TaskId=c.TaskId,
                        TagName=c.Tag!=null?c.Tag.TagName:null
                    }).ToList()
                }).FirstOrDefaultAsync(x => x.TaskId == query.TaskId, cancellationToken);



                if (task == null)
                {
                    return new Response<TaskDTO>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<TaskDTO>() { result = Result.Success(), data = task };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<TaskDTO>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
        private static string GetProcessedFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var parts = fileName.Split('-');
            return parts.Length > 1
                ? string.Join("-", parts.Skip(1))
                : fileName;
        }
    }

}
