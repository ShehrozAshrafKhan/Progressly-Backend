using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.ProjectAssignees.Commands.CreateProjectAssignee
{
    public class CreateProjectAssigneeCommandHandler : IRequestHandler<CreateProjectAssigneeCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateProjectAssigneeCommandHandler(IApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Response<bool>> Handle(CreateProjectAssigneeCommand command, CancellationToken cancellationToken)
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
                if (string.IsNullOrWhiteSpace(command.UserId))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "UserId not found!" }) };
                }
                var isUserExisted = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);
                if (isUserExisted == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "User not found!" }) };

                }
                var projectAssignee = new Domain.ProjectAssignee()
                {
                    ProjectId = isProjectExisted.ProjectId,
                    UserId = isUserExisted.Id
                };
                await _context.ProjectAssignees.AddAsync(projectAssignee);
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
