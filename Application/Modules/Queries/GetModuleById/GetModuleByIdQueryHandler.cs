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

namespace Application.Modules.Queries.GetModuleById
{
    public class GetModuleByIdQueryHandler : IRequestHandler<GetModuleByIdQuery, Response<ModuleDTO>>
    {
        private readonly IApplicationDbContext _context;

        public GetModuleByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<ModuleDTO>> Handle(GetModuleByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var module = await _context.Modules
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
               .FirstOrDefaultAsync(x=>x.ModuleId==query.ModuleId,cancellationToken);
                if (module == null)
                {
                    return new Response<ModuleDTO>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<ModuleDTO>() { result = Result.Success(), data = module };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<ModuleDTO>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
