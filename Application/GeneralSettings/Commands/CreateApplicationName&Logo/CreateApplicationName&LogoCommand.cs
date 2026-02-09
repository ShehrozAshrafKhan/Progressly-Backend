using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GeneralSettings.Commands.CreateApplicationName_Logo
{
    public class CreateApplicationName_LogoCommand:IRequest<Response<bool>>
    {
        public string? ApplicationName { get; set; }
        [FromForm(Name = "File")]
        public IFormFile? File { get; set; }
    }
}
