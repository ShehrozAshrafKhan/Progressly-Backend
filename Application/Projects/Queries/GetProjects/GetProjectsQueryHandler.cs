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

namespace Application.Projects.Queries.GetProjects
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, Response<List<Project>>>
    {
        private readonly IApplicationDbContext _context;

        public GetProjectsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<Project>>> Handle(GetProjectsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var projects = await _context.Projects.ToListAsync(cancellationToken);
                if (projects.Count == 0 || projects == null)
                {
                    return new Response<List<Project>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<List<Project>>() { result = Result.Success(), data = projects };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<Project>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
