using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Modules.DTOs;
using Application.Projects.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Queries.GetActiveModules
{
    public class GetActiveModulesQueryHandler : IRequestHandler<GetActiveModulesQuery, Response<List<ModuleDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetActiveModulesQueryHandler(IApplicationDbContext context)
        {
           _context = context;
        }
        public async Task<Response<List<ModuleDTO>>> Handle(GetActiveModulesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var modules = await _context.Modules.Where(x => x.IsActive == true).Select(x => new ModuleDTO()
                {
                    ProjectId = x.ProjectId,
                    Description = x.Description,
                    ModuleName = x.ModuleName,
                    ModuleId = x.ModuleId,
                }).ToListAsync(cancellationToken);

                if (modules.Count == 0 || modules == null)
                {
                    return new Response<List<ModuleDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<List<ModuleDTO>>() { result = Result.Success(), data = modules };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<ModuleDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
