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

namespace Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProjectCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.ProjectId == null || command.ProjectId == Guid.Empty)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ProjectId not found!" }) };
                }
                var isProjectExisted = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectId == command.ProjectId, cancellationToken);
                if (isProjectExisted == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "Project not found!" }) };

                }
                if (string.IsNullOrWhiteSpace(command.ProjectName))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ProjectName is Required!" }) };
                }
                isProjectExisted.ProjectName = command.ProjectName;
                isProjectExisted.StartDate = command.StartDate ?? DateTime.UtcNow;
                isProjectExisted.EndDate = command.EndDate ?? null;
                isProjectExisted.IsActive = command.IsActive;
                isProjectExisted.Description = command.Description;
                
                 _context.Projects.Update(isProjectExisted);
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
