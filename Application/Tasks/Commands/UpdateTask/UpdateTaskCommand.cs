using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand:IRequest<Response<Guid>>
    {
        public Guid? TaskId { get; set; }
        public string? Title { get; set; }
        public string? TaskNo { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public bool? IsActive { get; set; }
        public double? EstimatedHours { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ModuleId { get; set; }
    }
}
