using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskAttachments.Commands.CreateTaskAttachment
{
    public class CreateTaskAttachmentCommand:IRequest<Response<bool>>
    {
        [FromForm(Name="File")]
        public IFormFile? File { get; set; }
        public Guid? TaskId { get; set; }

    }
}
