

namespace Progressly.Application.DocumentStore.Commands.DeleteFile;
public class DeleteFileCommandHandler
{
    public DeleteFileCommandHandler()
    {
        
    }

    public bool Handle(DeleteFileCommand command)
    {
        try
        {
            foreach (var path in command.Paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.InnerException == null ? ex.Message : ex.InnerException.Message);
        }
    }
}
