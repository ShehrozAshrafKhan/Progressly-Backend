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

namespace Application.Tags.Queries.GetTagById
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Response<Tag>>
    {
        private readonly IApplicationDbContext _context;

        public GetTagByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<Tag>> Handle(GetTagByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(x => x.TagId == query.TagId, cancellationToken);
                if (tag == null)
                {
                    return new Response<Tag>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<Tag>() { result = Result.Success(), data = tag };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<Tag>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
