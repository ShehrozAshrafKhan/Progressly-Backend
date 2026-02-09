using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskAssignee.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAssignee.Queries.GetTaskAssigneeById
{
    public class GetTaskAssigneeByIdQueryHandler : IRequestHandler<GetTaskAssigneeByIdQuery, Response<TaskAssigneeDTO>>
    {
        private readonly IApplicationDbContext _context;

        public GetTaskAssigneeByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<TaskAssigneeDTO>> Handle(GetTaskAssigneeByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var taskAssignee = await _context.TaskAssignees.Include(x=>x.Task).Include(y=>y.AssignedByUser)
                    .Select(x=>new TaskAssigneeDTO
                    {
                        TaskAssigneeId = x.TaskAssigneeId,
                        AssignedAt = x.AssignedAt,
                        AssignedBy = x.AssignedByUser.Id,
                        AssignedUserName = x.AssignedByUser.UserName,
                        TaskId = x.TaskId,
                        TaskName=x.Task.Title
                    })
                    .FirstOrDefaultAsync(x => x.TaskAssigneeId == query.TaskAssigneeId, cancellationToken);
                if (taskAssignee == null)
                {
                    return new Response<TaskAssigneeDTO>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<TaskAssigneeDTO>() { result = Result.Success(), data = taskAssignee };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<TaskAssigneeDTO>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
