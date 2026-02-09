using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Module.Commands.CreateModule
{
    public class CreateModuleCommand:IRequest<Response<bool>>
    {
        public string? ModuleName { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; } = true;
        public Guid? ProjectId { get; set; }
    }
}
