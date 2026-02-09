using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.DTOs
{
    public class ModuleDTO
    {
        public Guid ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public Guid? ProjectId { get; set; }
        public bool? IsActive { get; set; }
    }
}
