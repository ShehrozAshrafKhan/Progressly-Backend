using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tags.Commands.UpdateTag
{
    public class UpdateTagCommand:IRequest<Response<bool>>
    {
        public Guid? TagId {  get; set; }   
        public string? TagNo { get; set; }
        public string? TagName { get; set; }
        public bool? IsActive { get; set; }
    }
}
