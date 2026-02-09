using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.Commands.DeleteTaskCodeChange
{
    public class DeleteTaskCodeChangeCommand:IRequest<Response<bool>>
    {
        public Guid? TaskCodeChangeId { get; set; }
    }
}
