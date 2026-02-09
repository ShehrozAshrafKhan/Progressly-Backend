using Application.Common.Models;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reports.Queries.GetProjects
{
    public class GetProjectsQuery:IRequest<Response<List<Project>>>
    {
    }
}
