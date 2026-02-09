using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? ProfilePath { get; set; }
        public string? ProfileName { get; set; }
        public bool? IsActive { get; set; }

    }
}
