using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressly.Application.DocumentStore.DTOs;
public class FileInfoDto
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required byte[] FileContent { get; set; }
    public required string FilePath { get; set; }
}
