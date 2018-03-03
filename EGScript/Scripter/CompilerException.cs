using System;

namespace EGScript.Scripter
{
    public class CompilerException : Exception
    {
        public CompilerException(string message) : base($"Compiler: {message}")
        {
        }
    }
}
