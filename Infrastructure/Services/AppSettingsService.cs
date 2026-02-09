using Application.Common.Interfaces;
using Application.Common.Models;
using Application.GeneralSettings.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AppSettingsService : IAppSettingsService
    {

        private readonly IApplicationDbContext _context;
        private readonly HttpContextAccessor _httpContextAccessor;

        public AppSettingsService(IApplicationDbContext context,HttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<GeneralSettingDTO>> GetAppSettingsAsync()
        {
            try
            {
                var generalSetting = await _context.GeneralSettings.Select(x => new GeneralSettingDTO
                {
                    LogoName = x.LogoName,
                    GeneralSettingId = x.GeneralSettingId,
                    LogoPath = x.LogoPath,
                    ApplicationName = x.ApplicationName,
                }).FirstOrDefaultAsync();
                if (generalSetting == null)
                {
                    return new Response<GeneralSettingDTO>() { result = Result.Failure(new List<string>() { "No Data Found!" }) };
                }
                return new Response<GeneralSettingDTO>() { result = Result.Success(), data = generalSetting };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<GeneralSettingDTO>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }

}
