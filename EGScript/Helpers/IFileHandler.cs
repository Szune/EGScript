namespace EGScript.Helpers
{
    public interface IFileHandler
    {
        string WorkingDirectory { get; }
        string ReadFileToEnd(string filePath);
        IFileHandler Copy(string newWorkingDir);
    }
}
