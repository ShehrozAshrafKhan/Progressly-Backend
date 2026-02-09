using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Modules.DTOs;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Queries.GetModules
{
    public class GetModulesQueryHandler : IRequestHandler<GetModulesQuery, Response<List<ModuleDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetModulesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<ModuleDTO>>> Handle(GetModulesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var modules = await _context.Modules
                .Include(x => x.Project) 
                .Select(x => new ModuleDTO
                {
                    Description = x.Description,
                    IsActive = x.IsActive,
                    ModuleId = x.ModuleId,
                    ModuleName = x.ModuleName,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project!.ProjectName
                })
                .ToListAsync(cancellationToken);

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
