using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public class LexerException : Exception
    {
        public LexerException(string message, int line, int character) : base($"Lexer: {message}{Environment.NewLine}At line '{line}', pos '{character}'.")
        {

        }
    }
}
