using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TaskCodeChanges.Commands.UploadCodeChanges
{
    public class UploadCodeChangesCommand : IRequest<Response<bool>>
    {
        public IFormCollection FormData { get; set; }
        public UploadCodeChangesCommand(IFormCollection formData)
        {
            FormData = formData;
        }
    }
}
