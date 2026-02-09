using Application.Common.Models;
using MediatR;
using Progressly.Application.DocumentStore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAttachments.Queries.GetTaskAttachmentFiles
{
    public class GetTaskAttachmentFileQuery:IRequest<Response<FileInfoDto>>
    {
        public Guid? TaskAttachmentId { get; set; }
    }
}
