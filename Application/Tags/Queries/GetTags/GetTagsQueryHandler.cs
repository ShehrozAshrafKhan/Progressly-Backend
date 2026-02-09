using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Queries.GetTags
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, Response<List<Domain.Tag>>>
    {
        private readonly IApplicationDbContext _context;

        public GetTagsQueryHandler(IApplicationDbContext context)
        {
             _context = context;
        }
        public async Task<Response<List<Domain.Tag>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tags = await _context.Tags.ToListAsync(cancellationToken);
                if (tags.Count==0||tags==null)
                {
                    return new Response<List<Domain.Tag>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<List<Domain.Tag>>() { result=Result.Success(),data=tags};

            }catch(Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<Domain.Tag>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
