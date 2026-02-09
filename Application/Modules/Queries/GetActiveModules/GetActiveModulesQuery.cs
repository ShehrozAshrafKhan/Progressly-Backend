using Application.Common.Models;
using Application.Modules.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Queries.GetActiveModules
{
    public class GetActiveModulesQuery:IRequest<Response<List<ModuleDTO>>>
    {
    }
}
