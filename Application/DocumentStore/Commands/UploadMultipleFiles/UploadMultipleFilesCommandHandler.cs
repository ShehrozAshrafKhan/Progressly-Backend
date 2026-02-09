using System;

namespace Progressly.Application.DocumentStore.Commands.UploadMultipleFiles;

public class UploadMultipleFilesCommandHandler

{

    private readonly string _uploadFolder;

    public UploadMultipleFilesCommandHandler()

    {

        _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

    }

    public async Task<List<string>> HandleAsync(UploadMultipleFilesCommand command)

    {

        var uploadedFiles = new List<string>();

        foreach (var file in command.Files)

        {

            if (file.Length > 0)

            {

                var filePath = Path.Combine(_uploadFolder, file.FileName);

                // Save each file to the server

                using var stream = new FileStream(filePath, FileMode.Create);

                await file.CopyToAsync(stream);

                uploadedFiles.Add(file.FileName);

            }

        }

        return uploadedFiles;

    }

}

