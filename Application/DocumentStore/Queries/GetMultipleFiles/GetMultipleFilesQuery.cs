using System;

namespace Progressly.Application.DocumentStore.Queries.GetMultipleFiles;

public class GetMultipleFilesQuery
{
    public List<string> FileNames { get; set; } = default!;
}
