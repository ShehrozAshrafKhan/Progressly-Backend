using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Commands.CreateTags
{
    public class CreateTagsCommandHandler : IRequestHandler<CreateTagsCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;

        public CreateTagsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<bool>> Handle(CreateTagsCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var tag = new Domain.Tag()
                {
                     TagName=command.TagName,
                     IsActive=command.IsActive,
                };

                await _context.Tags.AddAsync(tag);
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
