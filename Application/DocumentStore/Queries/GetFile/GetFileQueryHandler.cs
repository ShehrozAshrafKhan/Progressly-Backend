//using Progressly.Application.DocumentStore.DTOs;
//namespace Progressly.Application.DocumentStore.Queries.GetFile;

//public class GetFileQueryHandler
//{
//    public GetFileQueryHandler() { }
//    public async Task<FileInfoDto> Handle(GetFileQuery query)
//    {
//        var fileByte = await File.ReadAllBytesAsync(query.FilePath);
//        //var fileName = query.FilePath.Split("UploadedFiles\\")[1];
//        var contentType = GetContentType(query.FilePath);
//        return new FileInfoDto { FileContent = fileByte, ContentType = contentType, FileName = query.FileName, FilePath = query.FilePath };
//    }
//    private string GetContentType(string filePath)
//    {
//        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
//        return provider.TryGetContentType(filePath, out var contentType) ? contentType : "application/octet-stream";
//    }
//}

using Progressly.Application.DocumentStore.DTOs;
using Microsoft.AspNetCore.StaticFiles;

namespace Progressly.Application.DocumentStore.Queries.GetFile;

public class GetFileQueryHandler
{
    private readonly string _baseDirectory;

    public GetFileQueryHandler()
    {
        // You can make this configurable or inject it if needed
        _baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
    }

    public async Task<FileInfoDto?> Handle(GetFileQuery query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query.FilePath))
                return null;

            var safePath = query.FilePath.TrimStart('/')
                                         .Replace("/", Path.DirectorySeparatorChar.ToString());

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), safePath);

            // Optional: restrict access to only within UploadedFiles
            if (!fullPath.StartsWith(_baseDirectory))
            {
                // Reject suspicious paths
                return null;
            }

            if (!File.Exists(fullPath))
            {
                return null;
            }

            var fileBytes = await File.ReadAllBytesAsync(fullPath);
            var contentType = GetContentType(fullPath);

            return new FileInfoDto
            {
                FileContent = fileBytes,
                ContentType = contentType,
                FileName = query.FileName,
                FilePath = query.FilePath
            };
        }
        catch
        {
            // You can log the exception here if needed
            return null;
        }
    }

    private string GetContentType(string filePath)
    {
        var provider = new FileExtensionContentTypeProvider();
        return provider.TryGetContentType(filePath, out var contentType)
            ? contentType
            : "application/octet-stream";
    }
}
