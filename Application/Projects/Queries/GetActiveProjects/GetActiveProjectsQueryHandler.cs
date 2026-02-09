using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Projects.DTOs;
using Application.Tags.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Queries.GetActiveProjects
{
    public class GetActiveProjectsQueryHandler : IRequestHandler<GetActiveProjectsQuery, Response<List<ProjectsDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetActiveProjectsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<ProjectsDTO>>> Handle(GetActiveProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var projects = await _context.Projects.Where(x => x.IsActive == true).Select(x => new ProjectsDTO()
                {
                    ProjectId=x.ProjectId,
                    Description=x.Description,
                    EndDate=x.EndDate,
                    ProjectName=x.ProjectName,  
                    StartDate=x.StartDate
                }).ToListAsync(cancellationToken);

                if (projects.Count == 0 || projects == null)
                {
                    return new Response<List<ProjectsDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<List<ProjectsDTO>>() { result = Result.Success(), data = projects };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<ProjectsDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
