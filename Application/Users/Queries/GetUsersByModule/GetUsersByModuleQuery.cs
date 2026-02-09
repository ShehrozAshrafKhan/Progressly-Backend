using Application.Common.Models;
using Application.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUsersByModule
{
    public class GetUsersByModuleQuery:IRequest<Response<List<UserDTO>>>
    {
        public Guid? ModuleId { get; set; }
    }
}
