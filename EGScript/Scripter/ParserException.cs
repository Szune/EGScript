using System;

namespace EGScript.Scripter
{
    public class ParserException : Exception
    {
        public ParserException(string message, int line, int character) : base($"Parser: {message}{Environment.NewLine}At line '{line}', pos '{character}'.")
        {
        }
    }
}
