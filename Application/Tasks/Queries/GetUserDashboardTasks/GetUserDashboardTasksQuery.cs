using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.GetUserDashboardTasks
{
    public class GetUserDashboardTasksQuery:IRequest<Response<List<Dictionary<string,int>>>>
    {
    }
}
