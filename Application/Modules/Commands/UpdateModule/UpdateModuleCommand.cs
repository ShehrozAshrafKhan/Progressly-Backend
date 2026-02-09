using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Commands.UpdateModule
{
    public class UpdateModuleCommand:IRequest<Response<bool>>
    {
        public Guid? ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
