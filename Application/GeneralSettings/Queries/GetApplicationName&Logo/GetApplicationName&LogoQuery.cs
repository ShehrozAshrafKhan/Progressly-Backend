using Application.Common.Models;
using Application.GeneralSettings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GeneralSettings.Queries.GetApplicationName_Logo
{
    public class GetApplicationName_LogoQuery:IRequest<Response<GeneralSettingDTO>>
    {
    }
}
