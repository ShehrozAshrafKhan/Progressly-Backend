using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Tags.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Queries.GetActiveTags
{
    public class GetActiveTagsQueryHandler : IRequestHandler<GetActiveTagsQuery, Response<List<TagsDTO>>>
    {
        private readonly IApplicationDbContext _context;

        public GetActiveTagsQueryHandler(IApplicationDbContext context ) {
           _context = context;
        }
        public async Task<Response<List<TagsDTO>>> Handle(GetActiveTagsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var tags = await _context.Tags.Where(x=>x.IsActive==true).Select(x=>new TagsDTO() 
                {   TagId= x.TagId,
                    TagName= x.TagName,
                }).ToListAsync();

                if (tags.Count == 0 || tags == null)
                {
                    return new Response<List<TagsDTO>>() { result = Result.Failure(new List<string>() { "No Data Found" }) };
                }
                return new Response<List<TagsDTO>>() { result = Result.Success(), data = tags };

            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<List<TagsDTO>>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
