using System;
using Microsoft.AspNetCore.Http;

namespace Progressly.Application.DocumentStore.Commands.UploadFile;

public class UploadFileCommand
{
    public IFormFile? File { get; set; } = default!;
}
