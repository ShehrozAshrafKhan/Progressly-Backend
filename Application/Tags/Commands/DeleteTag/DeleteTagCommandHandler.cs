using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Commands.DeleteTag
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteTagCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(x => x.TagId == command.TagId,cancellationToken);
                if (tag == null) {
                    return new Response<bool>() { result = Result.Failure(new List<string> { "Tag not found!" }) };
                }
                 _context.Tags.Remove(tag);
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>() { result=Result.Success(),data=true};
            }catch(Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
