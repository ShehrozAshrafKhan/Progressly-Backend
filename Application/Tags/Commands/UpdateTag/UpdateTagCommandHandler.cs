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

namespace Application.Tags.Commands.UpdateTag
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTagCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.TagId == null || command.TagId == Guid.Empty)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "TagId not found!" }) };
                }
                var isTagExisted = await _context.Tags.FirstOrDefaultAsync(x => x.TagId == command.TagId, cancellationToken);
                if (isTagExisted == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "Tag not found!" }) };

                }
                if (string.IsNullOrWhiteSpace(command.TagName))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "TagName is Required!" }) };
                }
                isTagExisted.TagName = command.TagName;
                isTagExisted.IsActive = command.IsActive;

                _context.Tags.Update(isTagExisted);
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
