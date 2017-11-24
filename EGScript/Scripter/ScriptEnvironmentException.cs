using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGScript.Scripter
{
    public class ScriptEnvironmentException : Exception
    {
        public ScriptEnvironmentException(string message) : base($"Environment: {message}")
        {
        }
    }
}
