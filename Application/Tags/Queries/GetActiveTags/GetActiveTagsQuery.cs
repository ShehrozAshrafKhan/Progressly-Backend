using Application.Common.Models;
using Application.Tags.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Queries.GetActiveTags
{
    public class GetActiveTagsQuery:IRequest<Response<List<TagsDTO>>>
    {
    }
}
