using System;
using Microsoft.AspNetCore.Http;

namespace Progressly.Application.DocumentStore.Commands.UploadMultipleFiles;

public class UploadMultipleFilesCommand
{
    public IFormFileCollection Files { get; set; } = default!;
}
