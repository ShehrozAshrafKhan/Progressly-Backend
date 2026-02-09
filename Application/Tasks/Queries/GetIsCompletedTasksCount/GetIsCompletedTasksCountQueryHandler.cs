using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.GetIsCompletedTasksCount
{
    public class GetIsCompletedTasksCountQueryHandler : IRequestHandler<GetIsCompletedTasksCountQuery, Response<int>>
    {
        private readonly IApplicationDbContext _context;

        public GetIsCompletedTasksCountQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<int>> Handle(GetIsCompletedTasksCountQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var totalPendingApprovalTasks = await _context.Tasks.Where(x=>x.IsCompletedRequest==true&&x.IsActive==true).CountAsync(cancellationToken);
                return new Response<int>() { result = Result.Success(), data = totalPendingApprovalTasks };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<int>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
