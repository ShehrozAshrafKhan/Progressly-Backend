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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Modules.Commands.UpdateModule
{
    public class UpdateModuleCommandHandler : IRequestHandler<UpdateModuleCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateModuleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(UpdateModuleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.ModuleId == null || command.ModuleId == Guid.Empty)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ModuleId not found!" }) };
                }
                var isModuleExisted = await _context.Modules.FirstOrDefaultAsync(x => x.ModuleId == command.ModuleId, cancellationToken);
                if (isModuleExisted == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "Module not found!" }) };
                }
                if (command.ProjectId == null || command.ProjectId == Guid.Empty)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ProjectId not found!" }) };
                }
                var isProjectExisted = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectId == command.ProjectId, cancellationToken);
                if (isProjectExisted == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "Project not found!" }) };
                }
                if (string.IsNullOrWhiteSpace(command.ModuleName))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ModuleName is Required!" }) };
                }
                isModuleExisted.ModuleName = command.ModuleName;
                isModuleExisted.IsActive = command.IsActive;
                isModuleExisted.Description = command.Description;
                isModuleExisted.ProjectId = isProjectExisted.ProjectId;

                _context.Modules.Update(isModuleExisted);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>() { result = Result.Success(), data = true };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
