using Application.Common.Models;
using Application.Projects.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Queries.GetActiveProjects
{
    public class GetActiveProjectsQuery:IRequest<Response<List<ProjectsDTO>>>
    {
    }
}
