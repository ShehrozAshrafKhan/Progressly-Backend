using Application.Common.Interfaces;
using Application.Common.Models;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Progressly.Application.DocumentStore.Commands.UploadFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.GeneralSettings.Commands.CreateApplicationName_Logo
{
    public class CreateApplicationName_LogoCommandHandler:IRequestHandler<CreateApplicationName_LogoCommand,Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UploadFileCommandHandler _uploadFileCommandHandler;

        public CreateApplicationName_LogoCommandHandler(IApplicationDbContext context,UploadFileCommandHandler uploadFileCommandHandler)
        {
           _context = context;
           _uploadFileCommandHandler = uploadFileCommandHandler;
        }

        public async Task<Response<bool>> Handle(CreateApplicationName_LogoCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.ApplicationName))
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "ApplicationName is required!" }) };
                }
                if (command.File == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "File is null or Empty" }) };
                }
                var fileResult = await _uploadFileCommandHandler.HandleAsync(new UploadFileCommand { File = command.File });
                if (fileResult.Item1 == null)
                {
                    return new Response<bool>() { result = Result.Failure(new List<string>() { "File upload failed!" }) };
                }
                var isExist = await _context.GeneralSettings.FirstOrDefaultAsync();
                if (isExist==null)
                {
                    var generalSetting = new GeneralSetting()
                    {
                        ApplicationName = command.ApplicationName,
                        LogoPath = fileResult.Item2 ?? "",
                        LogoName = fileResult.Item1
                    };
                   await _context.GeneralSettings.AddAsync(generalSetting);
                }
                else
                {
                    isExist.ApplicationName = command.ApplicationName;
                    isExist.LogoPath = fileResult.Item2 ?? "";
                    isExist.LogoName = fileResult.Item1;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return new Response<bool>() { result = Result.Success(), data = true };
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message == null ? ex.InnerException.ToString() : ex.InnerException.Message;
                return new Response<bool>() { result = Result.Failure(new List<string>() { message }) };
            }
        }
    }
}
