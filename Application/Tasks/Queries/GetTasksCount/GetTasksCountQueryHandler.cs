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

namespace Application.Tasks.Queries.GetTasksCount
{
    public class GetTasksCountQueryHandler : IRequestHandler<GetTasksCountQuery, Response<int>>
    {
        private readonly IApplicationDbContext _context;

        public GetTasksCountQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<int>> Handle(GetTasksCountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var totalTasks = await _context.Tasks.Where(x=>x.IsActive==true).CountAsync(cancellationToken);
                return new Response<int>() { result = Result.Success(), data = totalTasks };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<int>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
