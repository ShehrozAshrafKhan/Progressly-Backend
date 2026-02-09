using Application.Common.Models;
using Application.GeneralSettings.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
        public interface IAppSettingsService
        {
            Task<Response<GeneralSettingDTO>> GetAppSettingsAsync();
        }
}
