using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand:IRequest<Response<Guid>>
    {
        public string? Title { get; set; } = null!;
        public string? TaskNo { get; set; } = null;
        public string? Description { get; set; }
        public string? Status { get; set; } = "PENDING";
        public string? Priority { get; set; }
        public bool? IsActive { get; set; } = true;
        public double? EstimatedHours { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ModuleId { get; set; }
    }
}
