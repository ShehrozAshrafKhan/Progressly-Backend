using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Commands.DeleteModule
{
    public class DeleteModuleCommand:IRequest<Response<bool>>
    {
        public Guid? ModuleId { get; set; }
    }
}
