using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Progressly.Application.DocumentStore.Commands.UploadFile
{
    public class UploadFileCommandHandler
    {
        public UploadFileCommandHandler()
        {

        }

        public async Task<(string?, string?)> HandleAsync(UploadFileCommand command)
        {
            try
            {
                if (command.File == null || command.File.Length == 0)
                {
                    return (null, "File does not exist!");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfffff}-{Path.GetFileName(command.File.FileName)}";

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await command.File.CopyToAsync(stream);
                }

                return (fileName, filePath);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
}

