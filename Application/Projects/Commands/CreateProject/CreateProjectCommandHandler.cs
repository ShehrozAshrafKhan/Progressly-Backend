using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public CreateProjectCommandHandler(IApplicationDbContext context)
        {
           _context = context;
        }
        public async Task<Response<bool>> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.ProjectName))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ProjectName is Required!" }) };
                }
                var project = new Project()
                {
                    ProjectName = command.ProjectName,
                    StartDate = command.StartDate ?? DateTime.UtcNow,
                    EndDate = command.EndDate ?? null,
                    IsActive = command.IsActive,
                    Description = command.Description
                };
                await _context.Projects.AddAsync(project);
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
