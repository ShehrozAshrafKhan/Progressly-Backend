//using Application.Common.Interfaces;
//using Application.Services;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Services
//{
//    public class FileService: IFileService
//    {
//        private readonly IWebHostEnvironment _env;
//        private readonly IApplicationDbContext _context;

//        public FileService(IWebHostEnvironment env,IApplicationDbContext context)
//        {
//            _env = env;
//            _context = context;
//        }

//        public async Task<string> SaveFileAsync(IFormFile file, string subFolder="Uploads/Tasks")
//        {
//            var uploadsFolder = Path.Combine(_env.WebRootPath, subFolder);
//            if (!Directory.Exists(uploadsFolder))
//                Directory.CreateDirectory(uploadsFolder);

//            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
//            var uniqueId = Guid.NewGuid().ToString();
//            var uniqueFileName = $"{timestamp}_{uniqueId}_{file.FileName}";
//            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

//            using var stream = new FileStream(fullPath, FileMode.Create);
//            await file.CopyToAsync(stream);

//            return $"/{subFolder}/{uniqueFileName}".Replace("\\", "/");
//        }

//        public async Task<(byte[] FileBytes, string ContentType, string FileName)> ReadFileAsync(Guid attachmentId)
//        {
//            var attachment = await _context.TaskAttachments.FindAsync(attachmentId);

//            if (attachment == null)
//                throw new FileNotFoundException("Attachment record not found in database.");

//            if (string.IsNullOrWhiteSpace(attachment.FilePath))
//                throw new FileNotFoundException("No file path found in attachment record.");

//            // Clean and combine full path
//            var trimmedPath = attachment.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
//            var fullPath = Path.Combine(_env.WebRootPath, trimmedPath);

//            if (!File.Exists(fullPath))
//                throw new FileNotFoundException("File does not exist on disk.");

//            var bytes = await File.ReadAllBytesAsync(fullPath);
//            var contentType = GetContentType(fullPath);

//            return (bytes, contentType, attachment.FileName);
//        }

//        private string GetContentType(string path)
//        {
//            var ext = Path.GetExtension(path).ToLowerInvariant();
//            return ext switch
//            {
//                ".pdf" => "application/pdf",
//                ".jpg" or ".jpeg" => "image/jpeg",
//                ".png" => "image/png",
//                ".txt" => "text/plain",
//                ".doc" => "application/msword",
//                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
//                _ => "application/octet-stream"
//            };
//        }

//    }
//}
