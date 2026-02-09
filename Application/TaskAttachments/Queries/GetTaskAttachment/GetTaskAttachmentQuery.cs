using Application.Common.Models;
using Application.TaskAttachments.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAttachments.Queries.GetTaskAttachment
{
    public class GetTaskAttachmentQuery:IRequest<Response<List<TaskAttachmentDTO>>>
    {
    }
}
