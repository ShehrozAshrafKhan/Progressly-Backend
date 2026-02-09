using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommand:IRequest<Response<bool>>
    {
        public Guid? TaskId { get; set; }
        public bool? IsCompletedRequest { get; set; }
    }
}
