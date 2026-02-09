using Application.Common.Models;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.Queries.GetTags
{
    public class GetTagsQuery:IRequest<Response<List<Domain.Tag>>>
    {
    }
}
