using Application.Common.Interfaces;
using Application.Common.Models;
using Application.GeneralSettings.DTOs;
using Application.TaskAttachments.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GeneralSettings.Queries.GetApplicationName_Logo
{
    public class GetApplicationName_LogoQueryHandler : IRequestHandler<GetApplicationName_LogoQuery, Response<GeneralSettingDTO>>
    {
        private readonly IApplicationDbContext _context;

        public GetApplicationName_LogoQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GeneralSettingDTO>> Handle(GetApplicationName_LogoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var generalSetting = await _context.GeneralSettings.Select(x => new GeneralSettingDTO
                {
                    LogoName = x.LogoName,
                    GeneralSettingId = x.GeneralSettingId,
                    LogoPath = x.LogoPath,
                    ApplicationName=x.ApplicationName,
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
