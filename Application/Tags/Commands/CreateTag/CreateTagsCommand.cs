using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Commands.CreateTags
{
    public class CreateTagsCommand:IRequest<Response<bool>>
    {
        public string? TagNo { get; set; }
        public string? TagName { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
