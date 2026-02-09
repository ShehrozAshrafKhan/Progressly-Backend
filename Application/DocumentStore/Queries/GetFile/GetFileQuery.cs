using System;

namespace Progressly.Application.DocumentStore.Queries.GetFile;

public class GetFileQuery
{
    public string FilePath { get; set; } = default!;
    public string FileName { get; set; } = default!;
}
