using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GeneralSettings.DTOs
{
    public class GeneralSettingDTO
    {
        public Guid GeneralSettingId { get; set; }
        public string? ApplicationName { get; set; }
        public string? LogoName { get; set; }
        public string? LogoPath { get; set; }
    }
}
