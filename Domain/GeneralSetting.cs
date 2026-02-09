using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class GeneralSetting:BaseAuditableEntity
    {
        [Key]
        public Guid GeneralSettingId { get; set; }
        public string? ApplicationName { get; set; }
        public string? LogoPath { get; set; }
        public string? LogoName { get; set; }

    }
}
