using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressly.Application.DocumentStore.Commands.DeleteFile;
public class DeleteFileCommand
{
    public required List<string> Paths { get; set; }
}
