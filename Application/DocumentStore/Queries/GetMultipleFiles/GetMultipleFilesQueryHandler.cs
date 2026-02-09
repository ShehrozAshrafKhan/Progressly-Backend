using System;

using System.IO.Compression;

namespace Progressly.Application.DocumentStore.Queries.GetMultipleFiles;

public class GetMultipleFilesQueryHandler

{

    private readonly string _uploadFolder;

    public GetMultipleFilesQueryHandler()

    {

        _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

    }

    public MemoryStream Handle(GetMultipleFilesQuery query)

    {

        var memoryStream = new MemoryStream();

        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))

        {

            foreach (var fileName in query.FileNames)

            {

                var filePath = Path.Combine(_uploadFolder, fileName);

                if (!File.Exists(filePath))

                    throw new FileNotFoundException($"File {fileName} not found");

                var entry = zipArchive.CreateEntry(fileName);

                using (var entryStream = entry.Open())

                using (var fileStream = new FileStream(filePath, FileMode.Open))

                {

                    fileStream.CopyTo(entryStream);

                }

            }

        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;

    }

}

