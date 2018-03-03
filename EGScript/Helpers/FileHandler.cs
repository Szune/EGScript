using System.IO;

namespace EGScript.Helpers
{
    public class FileHandler : IFileHandler
    {
        public FileHandler()
        {
            WorkingDirectory = "";
        }

        public FileHandler(string workingDir)
        {
            WorkingDirectory = workingDir;
        }

        public string WorkingDirectory { get; private set; }

        public IFileHandler Copy(string newWorkingDir)
        {
            return new FileHandler(newWorkingDir);
        }

        public string ReadFileToEnd(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        private string CleanPath(string path) => Path.GetDirectoryName(path);

    }
}
