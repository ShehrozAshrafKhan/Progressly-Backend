using Application.Common.Models;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskTags.Commands.CreateTaskTag
{
    public class CreateTaskTagCommand:IRequest<Response<bool>>
    {
        public Guid? TaskId { get; set; }
        public List<Guid>? TagIds { get; set; }
    }
}
