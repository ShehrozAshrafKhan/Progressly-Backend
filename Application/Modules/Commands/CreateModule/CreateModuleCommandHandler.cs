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

namespace Application.Module.Commands.CreateModule
{
    public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public CreateModuleCommandHandler(IApplicationDbContext context)
        {
           _context = context;
        }
        public async Task<Response<bool>> Handle(CreateModuleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.ModuleName))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ModuleName is Required!" }) };
                }
                if (command.ProjectId==null||command.ProjectId==Guid.Empty)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ProjectId not found!" }) };
                }
                var isProjectExisted = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectId == command.ProjectId, cancellationToken);
                if (isProjectExisted==null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "Project not found!" }) };

                }
                var module = new Domain.Module()
                {
                    ModuleName = command.ModuleName,
                    IsActive = command.IsActive,
                    Description = command.Description,
                    ProjectId=isProjectExisted.ProjectId
                };
                await _context.Modules.AddAsync(module);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>() { result = Result.Success(), data = true };

            }
            catch (Exception ex) {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
