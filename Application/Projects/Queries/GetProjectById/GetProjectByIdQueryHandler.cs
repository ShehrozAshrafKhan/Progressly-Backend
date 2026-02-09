using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Response<Project>>
    {
        private readonly IApplicationDbContext _context;

        public GetProjectByIdQueryHandler(IApplicationDbContext context)
        {
           _context = context;
        }
        public async Task<Response<Project>> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectId == query.ProjectId, cancellationToken);
                if (project == null)
                {
                    return new Response<Project>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<Project>() { result = Result.Success(), data = project };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<Project>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
