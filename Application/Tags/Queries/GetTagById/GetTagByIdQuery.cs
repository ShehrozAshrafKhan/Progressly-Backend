using Application.Common.Models;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Queries.GetTagById
{
    public class GetTagByIdQuery:IRequest<Response<Tag>>
    {
        public Guid? TagId { get; set; }
    }
}
