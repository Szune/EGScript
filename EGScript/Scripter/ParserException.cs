using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public class ParserException : Exception
    {
        public ParserException(string message, int line, int character) : base($"Parser: {message}{Environment.NewLine}At line '{line}', pos '{character}'.")
        {
        }
    }
}
