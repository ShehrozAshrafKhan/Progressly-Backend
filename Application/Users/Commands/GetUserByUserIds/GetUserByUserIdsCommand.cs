using Application.Common.Models;
using Application.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands.GetUserByUserIds
{
    public class GetUserByUserIdsCommand:IRequest<Response<List<UserDTO>>>
    {
        public Guid? ModuleId { get; set; }
        public List<string>? UserIds {  get; set; } 
    }
}
