using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Commands.DeleteModule
{
    public class DeleteModuleCommandHandler : IRequestHandler<DeleteModuleCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteModuleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(DeleteModuleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var module = await _context.Modules.FirstOrDefaultAsync(x => x.ModuleId == command.ModuleId, cancellationToken);
                if (module == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string> { "Module not found!" }) };
                }
                _context.Modules.Remove(module);
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
