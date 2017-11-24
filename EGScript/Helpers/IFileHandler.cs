using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Helpers
{
    public interface IFileHandler
    {
        string WorkingDirectory { get; }
        string ReadFileToEnd(string filePath);
        IFileHandler Copy(string newWorkingDir);
    }
}
