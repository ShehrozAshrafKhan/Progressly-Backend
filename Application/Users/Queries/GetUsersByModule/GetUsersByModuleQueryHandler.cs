using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TaskAttachments.DTOs;
using Application.Users.DTOs;
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

namespace Application.Users.Queries.GetUsersByModule
{
    public class GetUsersByModuleQueryHandler : IRequestHandler<GetUsersByModuleQuery, Response<List<UserDTO>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUsersByModuleQueryHandler(IApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Response<List<UserDTO>>> Handle(GetUsersByModuleQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (query.ModuleId == null || query.ModuleId == Guid.Empty)
                {
                    return new Response<List<UserDTO>>() { result = Result.Failure(new List<string>() { "ModuleId not found!" }) };
                }
                var isModuleExisted = await _context.Modules.FirstOrDefaultAsync(x => x.ModuleId == query.ModuleId, cancellationToken);
                if (isModuleExisted == null)
                {
                    return new Response<List<UserDTO>>() { result = Result.Failure(new List<string>() { "Module not found!" }) };
                }
                var users = await _context.ProjectAssignees
                .Where(pa => pa.ProjectId == isModuleExisted.ProjectId && pa.User.IsActive==true)
                .Include(pa => pa.User)
                .Select(pa => new UserDTO
                {
                    UserId = pa.User.Id,
                    UserName = pa.User.FullName,
                    Email = pa.User.Email,
                    IsActive = pa.User.IsActive
                })
                .ToListAsync(cancellationToken);

                if (users.Count == 0||users==null)
                {
                    return new Response<List<UserDTO>>() { result = Result.Failure(new List<string>() { "No User Found!" }) };
                }
                return new Response<List<UserDTO>>() { result=Result.Success(),data=users};
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<UserDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
